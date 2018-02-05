using SciAdvNet.SC3Script.Utils;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace SciAdvNet.SC3Script
{
    public sealed class SC3Disassembler : IDisposable
    {
        private static readonly string NamespaceName = typeof(SC3Disassembler).GetTypeInfo().Namespace;

        private readonly bool _leaveOpen;

        private SC3Disassembler(SC3Script script, bool leaveOpen)
        {
            Script = script;
            _leaveOpen = leaveOpen;
        }

        public SC3Script Script { get; }
        private BinaryReader Reader => Script.Reader;

        private int Position
        {
            get => (int)Script.Stream.Position;
            set => Script.Stream.Position = value;
        }

        public static SC3Disassembler Open(SC3Script script, bool leaveOpen = false)
        {
            if (script == null)
            {
                throw new ArgumentNullException(nameof(script));
            }

            return new SC3Disassembler(script, leaveOpen);
        }

        public static SC3Disassembler Load(Stream stream, string fileName, SC3Game game, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var script = SC3Script.Load(stream, fileName, game, leaveOpen);
            return new SC3Disassembler(script, leaveOpen);
        }

        public static SC3Disassembler Load(string path, SC3Game game)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var script = SC3Script.Load(path, game);
            return Open(script);
        }

        public DisassemblyResult DisassembleCodeBlock(CodeBlockDefinition blockDefinition)
        {
            Position = blockDefinition.Offset;
            var instructions = ImmutableArray.CreateBuilder<Instruction>();
            var refCollector = new ReferenceCollector(Script);
            ImmutableArray<byte> unrecognizedBytes = ImmutableArray<byte>.Empty;
            do
            {
                Instruction current = null;
                try
                {
                    current = NextInstruction();
                    if (current is EndOfScriptInstruction)
                    {
                        break;
                    }

                    if (instructions.Count > 0)
                    {
                        var last = instructions[instructions.Count - 1];
                        current.Previous = last;
                        last.Next = current;
                    }
                }
                catch (UnrecognizedInstructionException)
                {
                    unrecognizedBytes = Reader.ReadBytes(blockDefinition.EndOffset - Position).ToImmutableArray();
                    break;
                }

                instructions.Add(current);
                //refCollector.Visit(current);
            } while (Position < blockDefinition.EndOffset);

            return new DisassemblyResult(blockDefinition)
            {
                Instructions = instructions.ToImmutable(),
                UnrecognizedBytes = unrecognizedBytes,
                StringReferences = refCollector.StringReferences,
                CodeBlockReferences = refCollector.CodeBlockReferences,
                ExternalCodeBlockReferences = refCollector.ExternalCodeBlockReferences,
                DataBlockReferences = refCollector.DataBlockReferences
            };
        }

        private Instruction NextInstruction()
        {
            InstructionStub instructionStub = null;
            int offset = Position;

            instructionStub = RecognizeInstruction();

            string typeName = $"{NamespaceName}.{instructionStub.Name}{nameof(Instruction)}";
            var type = Type.GetType(typeName);
            if (type == null)
            {
                type = typeof(GenericInstruction);
            }

            var instruction = (Instruction)Activator.CreateInstance(type);
            instruction.Name = instructionStub.Name;
            instruction.Opcode = instructionStub.Opcode;

            var operands = ImmutableArray.CreateBuilder<Operand>();
            var typeInfo = instruction.GetType().GetTypeInfo();
            foreach (var operandStub in instructionStub.OperandStubs)
            {
                var operand = ReadOperand(operandStub);
                operand.Name = operandStub.Name;
                operands.Add(operand);

                if (!(instruction is GenericInstruction))
                {
                    var propertyInfo = typeInfo.GetDeclaredProperty(operandStub.Name);
                    if (propertyInfo != null)
                    {
                        typeInfo.GetDeclaredProperty(operandStub.Name).SetValue(instruction, operand);
                    }
                }
            }

            instruction.Offset = offset;
            instruction.Length = Position - offset;

            Reader.BaseStream.Position = offset;
            var bytes = Reader.ReadBytes(instruction.Length);
            instruction.Bytes = bytes.ToImmutableArray();
            instruction.Operands = operands.ToImmutable();
            return instruction;
        }

        private InstructionStub RecognizeInstruction()
        {
            var opcodeTable = Script.GameSpecificData.OpcodeTable;

            InstructionStub stub = null;
            byte[] opcode;
            int opcodeLength = 1;
            do
            {
                opcode = Reader.PeekBytes(opcodeLength);
                stub = opcodeTable.GetValueOrDefault(opcode.ToImmutableArray());
                if (stub == null)
                {
                    opcodeLength++;
                }
            } while (stub == null && opcodeLength <= 4);

            if (stub == null)
            {
                throw new UnrecognizedInstructionException();
            }

            Position += opcodeLength;
            return stub;
        }

        private Operand ReadOperand(OperandStub stub)
        {
            if (stub.IsPrimitiveType)
            {
                return ReadPrimitiveTypeValue(stub.Type);
            }

            return SC3ExpressionParser.ParseExpression(Reader);
        }

        private PrimitiveTypeValue ReadPrimitiveTypeValue(OperandType type)
        {
            switch (type)
            {
                case OperandType.Byte: return new PrimitiveTypeValue(Reader.ReadByte(), type);
                case OperandType.UInt16: return new PrimitiveTypeValue(Reader.ReadUInt16(), type);
                case OperandType.UInt32: return new PrimitiveTypeValue(Reader.ReadUInt32(), type);
                default: throw new ArgumentException(nameof(type));
            }
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                Script.Dispose();
            }
        }
    }
}

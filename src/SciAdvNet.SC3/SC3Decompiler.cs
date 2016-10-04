using SciAdvNet.Common;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace SciAdvNet.SC3
{
    public sealed class SC3Decompiler : IDisposable
    {
        private static readonly string NamespaceName = typeof(SC3Decompiler).GetTypeInfo().Namespace;

        private readonly bool _leaveOpen;

        private SC3Decompiler(SC3Module module, bool leaveOpen)
        {
            Module = module;
            _leaveOpen = leaveOpen;
        }

        public SC3Module Module { get; }

        private Stream Stream => Module.ModuleStream;
        private BinaryReader ModuleReader => Module.ModuleReader;

        private int Position
        {
            get { return (int)Stream.Position; }
            set { Stream.Position = value; }
        }

        private ImmutableDictionary<ImmutableArray<byte>, InstructionStub> OpcodeTable =>
            Module.GameSpecificData.OpcodeTable;

        public static SC3Decompiler Open(SC3Module module, bool leaveOpen = false)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            return new SC3Decompiler(module, leaveOpen);
        }

        public static SC3Decompiler Load(Stream stream, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var module = SC3Module.Load(stream, leaveOpen);
            return new SC3Decompiler(module, leaveOpen);
        }

        public static SC3Decompiler Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var module = SC3Module.Load(path);
            return Open(module);
        }

        public CodeBlock DecompileCodeBlock(CodeBlockDefinition blockDefinition)
        {
            Position = blockDefinition.Offset;
            var instrBuilder = ImmutableArray.CreateBuilder<Instruction>();
            var refCollector = new ReferenceCollector(Module);
            ImmutableArray<byte> unrecognizedBytes = ImmutableArray<byte>.Empty;
            do
            {
                Instruction currInstruction = null;
                try
                {
                    currInstruction = NextInstruction();
                    if (currInstruction is EndOfScriptInstruction)
                    {
                        break;
                    }

                    if (instrBuilder.Count > 0)
                    {
                        var last = instrBuilder[instrBuilder.Count - 1];
                        currInstruction.Previous = last;
                        last.Next = currInstruction;
                    }
                }
                catch (UnrecognizedInstructionException)
                {
                    unrecognizedBytes = ModuleReader.ReadBytes(blockDefinition.EndOffset - Position).ToImmutableArray();
                    break;
                }

                instrBuilder.Add(currInstruction);
                refCollector.Visit(currInstruction);
            } while (Position < blockDefinition.EndOffset);

            return new CodeBlock(blockDefinition)
            {
                Instructions = instrBuilder.ToImmutable(),
                UnrecognizedBytes = unrecognizedBytes,
                StringReferences = refCollector.Strings,
                CodeBlockReferences = refCollector.CodeBlocks,
                ExternalCodeBlockReferences = refCollector.ExternalCodeBlocks,
                DataBlockReferences = refCollector.DataBlocks
            };
        }

        private Instruction NextInstruction()
        {
            InstructionStub instrStub = null;
            int instrOffset = Position;

            instrStub = RecognizeInstruction();

            string typeName = $"{NamespaceName}.{instrStub.Name}{nameof(Instruction)}";
            var type = Type.GetType(typeName);
            if (type == null)
            {
                type = typeof(GenericInstruction);
            }

            var instruction = (Instruction)Activator.CreateInstance(type);
            instruction.Name = instrStub.Name;
            instruction.Opcode = instrStub.Opcode;

            var operandBuilder = ImmutableArray.CreateBuilder<Operand>();
            var typeInfo = instruction.GetType().GetTypeInfo();
            foreach (var operandStub in instrStub.OperandStubs)
            {
                var operand = ReadOperand(operandStub);
                operand.Name = operandStub.Name;
                operandBuilder.Add(operand);

                if (!(instruction is GenericInstruction))
                {
                    var propertyInfo = typeInfo.GetDeclaredProperty(operandStub.Name);
                    if (propertyInfo != null)
                    {
                        typeInfo.GetDeclaredProperty(operandStub.Name).SetValue(instruction, operand);
                    }
                }
            }

            instruction.Offset = instrOffset;
            instruction.Length = Position - instrOffset;
            instruction.Operands = operandBuilder.ToImmutable();
            return instruction;
        }

        private InstructionStub RecognizeInstruction()
        {
            InstructionStub instrStub = null;
            byte[] opcode;
            int opcodeLength = 1;
            do
            {
                opcode = ModuleReader.PeekBytes(opcodeLength);
                instrStub = OpcodeTable.GetValueOrDefault(opcode.ToImmutableArray());
                if (instrStub == null)
                {
                    opcodeLength++;
                }
            } while (instrStub == null && opcodeLength <= 3);

            if (instrStub == null)
            {
                throw new UnrecognizedInstructionException();
            }

            Position += opcodeLength;
            return instrStub;
        }

        private Operand ReadOperand(OperandStub stub)
        {
            if (stub.IsPrimitiveType)
            {
                return ReadPrimitiveTypeValue(stub.Type);
            }

            return SC3ExpressionParser.ParseExpression(ModuleReader);
        }

        private PrimitiveTypeValue ReadPrimitiveTypeValue(OperandType type)
        {
            switch (type)
            {
                case OperandType.Byte: return new PrimitiveTypeValue(ModuleReader.ReadByte(), type);
                case OperandType.UInt16: return new PrimitiveTypeValue(ModuleReader.ReadUInt16(), type);
                case OperandType.UInt32: return new PrimitiveTypeValue(ModuleReader.ReadUInt32(), type);
                default: throw new ArgumentException(nameof(type));
            }
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                Module.Dispose();
            }
        }
    }
}

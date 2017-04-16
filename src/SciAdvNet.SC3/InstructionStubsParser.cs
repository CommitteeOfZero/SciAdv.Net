using SciAdvNet.SC3.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SciAdvNet.SC3
{
    internal sealed class InstructionStubsParser
    {
        private const string RootElementName = "InstructionStubs";
        private const string InstructionStubElementName = "InstructionStub";
        private const string NameAttributeName = "Name";
        private const string OpcodeAttributeName = "Opcode";

        private static XNamespace RootNamespace = "http://schemas.sciadv.net/sc3/instructionstubs";
        private static readonly Dictionary<string, OperandType> s_operandTypeMap = new Dictionary<string, OperandType>()
        {
            ["expression"] = OperandType.Expression,
            ["expr"] = OperandType.Expression,
            ["byte"] = OperandType.Byte,
            ["uint16"] = OperandType.UInt16,
            ["ushort"] = OperandType.UInt16,
            ["uint32"] = OperandType.UInt32,
            ["uint"] = OperandType.UInt32
        };


        public static ImmutableDictionary<ImmutableArray<byte>, InstructionStub> Parse(Stream stream)
        {
            var doc = XDocument.Load(stream);
            var instructionStubElements = doc.Root.Elements(RootNamespace + InstructionStubElementName);
            return instructionStubElements.Select(ParseInstructionStubElement)
                .ToImmutableDictionary(x => x.Opcode, x => x, ByteArrayComparer.Instance);
        }

        private static InstructionStub ParseInstructionStubElement(XElement instructionStubElement)
        {
            string name = instructionStubElement.Attribute(NameAttributeName).Value;
            string strOpcode = instructionStubElement.Attribute(OpcodeAttributeName).Value;
            var opcode = BinaryUtils.HexStrToBytes(strOpcode);

            if (!string.IsNullOrEmpty(instructionStubElement.Value.Trim()))
            {
                string strInstructionStub = instructionStubElement.Value.Replace("\n", string.Empty).Trim();
                var operands = ParseInstructionStubString(strInstructionStub);
                return new InstructionStub(name, opcode.ToImmutableArray(), operands);
            }

            return new InstructionStub(name, opcode.ToImmutableArray());
        }

        private static ImmutableArray<OperandStub> ParseInstructionStubString(string strInstructionStub)
        {
            var result = ImmutableArray.CreateBuilder<OperandStub>();
            var strOperandStubs = strInstructionStub.Split(new[] { ", ", "," }, StringSplitOptions.None);
            foreach (var stub in strOperandStubs)
            {
                var parts = stub.Split(' ');
                Debug.Assert(parts.Length == 2);

                string strOperandType = parts[0];
                string operandName = parts[1];
                var operandType = s_operandTypeMap[strOperandType.ToLowerInvariant()];
                result.Add(new OperandStub(operandName, operandType));
            }

            return result.ToImmutable();
        }
    }
}

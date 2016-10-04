using System.Collections.Immutable;

namespace SciAdvNet.SC3
{
    public sealed class InstructionStub
    {
        internal InstructionStub(string name, ImmutableArray<byte> opcode, ImmutableArray<OperandStub> operandStubs)
        {
            Name = name;
            Opcode = opcode;
            OperandStubs = operandStubs;
        }

        internal InstructionStub(string name, ImmutableArray<byte> opcode)
            : this(name, opcode, ImmutableArray<OperandStub>.Empty)
        {
        }

        public string Name { get; }
        public ImmutableArray<byte> Opcode { get; }
        public ImmutableArray<OperandStub> OperandStubs { get; }

        public override string ToString() =>$"{Opcode}:{Name}";
    }
}

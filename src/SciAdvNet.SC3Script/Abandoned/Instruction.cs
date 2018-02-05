using System.Collections.Immutable;

namespace SciAdvNet.SC3Script
{
    public abstract class Instruction : IVisitable
    {
        internal Instruction()
        {
            Operands = ImmutableArray<Operand>.Empty;
        }

        public string Name { get; internal set; }
        public int Offset { get; internal set; }
        public int Length { get; internal set; }
        public ImmutableArray<byte> Bytes { get; internal set; }
        public ImmutableArray<byte> Opcode { get; internal set; }
        public ImmutableArray<Operand> Operands { get; internal set; }

        public Instruction Next { get; internal set; }
        public Instruction Previous { get; internal set; }

        public virtual void Accept(CodeVisitor visitor)
        {
        }

        public override string ToString() => Name;
    }

    public sealed class GenericInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.DefaultVisitInstruction(this);
        }
    }
}

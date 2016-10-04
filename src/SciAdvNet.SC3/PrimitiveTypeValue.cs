namespace SciAdvNet.SC3
{
    public sealed class PrimitiveTypeValue : Operand
    {
        internal PrimitiveTypeValue(uint value, OperandType type)
        {
            Value = value;
            Type = type;
        }

        public uint Value { get; }
        public OperandType Type { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitPrimitiveTypeValue(this);
        }
    }
}

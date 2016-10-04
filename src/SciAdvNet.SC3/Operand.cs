namespace SciAdvNet.SC3
{
    public abstract class Operand : IVisitable
    {
        public string Name { get; internal set; }

        public virtual void Accept(CodeVisitor visitor)
        {
        }
    }

    public enum OperandType
    {
        Expression,
        Byte,
        Boolean,
        UInt16,
        UInt32
    }
}

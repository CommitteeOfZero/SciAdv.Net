namespace SciAdvNet.SC3Script
{
    public sealed class OperandStub
    {
        internal OperandStub(string name, OperandType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public OperandType Type { get; }

        public bool IsPrimitiveType => Type != OperandType.Expression;
        public override string ToString() => $"{Type} {Name}";
    }
}

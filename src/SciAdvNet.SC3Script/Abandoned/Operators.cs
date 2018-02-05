namespace SciAdvNet.SC3Script
{
    public enum PrefixUnaryOperatorKind : byte
    {
        BitwiseNegation = 0x0B
    }

    public enum PostfixUnaryOperatorKind : byte
    {
        PostfixIncrement = 0x20,
        PostfixDecrement = 0x21
    }

    public enum BinaryOperatorKind : byte
    {
        Multiplication = 0x01,
        Division = 0x02,
        Addition = 0x03,
        Subtraction = 0x04,
        Modulo = 0x05,
        LeftShift = 0x06,
        RightShift = 0x07,
        BitwiseAnd = 0x08,
        BitwiseXor = 0x09,
        BitwiseOr = 0x0A,
        Equal = 0x0C,
        NotEqual = 0x0D,
        LessThanOrEqual = 0x0E,
        GreaterThanOrEqual = 0x0F,
        LessThan = 0x10,
        GreaterThan = 0x11
    }

    public enum AssignmentOperatorKind : byte
    {
        SimpleAssignment = 0x14,
        MultiplyAssignment = 0x15,
        DivideAssignment = 0x16,
        AddAssignment = 0x17,
        SubtractAssignment = 0x18,
        ModuloAssignment = 0x19,
        LeftShiftAssignment = 0x1A,
        RightShiftAssignment = 0x1B,
        AndAssignment = 0x1C,
        OrAssignment = 0x1D,
        ExclusiveOrAssignment = 0x1E,
    }

    internal enum SpecialOperatorKind : byte
    {
        GlobalVar = 0x28,
        Flag = 0x29,
        DataBlockAccess = 0x2A,
        DataBlockReference = 0x2B,
        ThreadLocalVar = 0x2D,
        RandomNumber = 0x33
    }
}

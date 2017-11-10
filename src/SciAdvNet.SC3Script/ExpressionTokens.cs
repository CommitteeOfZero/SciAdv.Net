namespace SciAdvNet.SC3Script
{
    internal abstract class ExpressionToken
    {
        protected ExpressionToken(byte precedence)
        {
            Precedence = precedence;
        }

        public byte Precedence { get; }
        public abstract ExpressionTokenKind Kind { get; }
        public virtual int Length { get; } = 2;
    }

    internal enum ExpressionTokenKind
    {
        Constant,
        PrefixUnaryOperator,
        PostfixUnaryOperator,
        BinaryOperator,
        AssignmentOperator,
        SpecialOperator,
        EndOfExpression
    }

    internal sealed class ConstantValueToken : ExpressionToken
    {
        public ConstantValueToken(int value, byte precedence, int length)
            : base(precedence)
        {
            Value = value;
            Length = length;
        }

        public int Value { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.Constant;
        public override int Length { get; }
    }

    internal sealed class PrefixUnaryOperatorToken : ExpressionToken
    {
        public PrefixUnaryOperatorToken(PrefixUnaryOperatorKind unaryOperator, byte precedence)
            : base(precedence)
        {
            Operator = unaryOperator;
        }

        public PrefixUnaryOperatorKind Operator { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.PrefixUnaryOperator;
    }

    internal sealed class PostfixUnaryOperatorToken : ExpressionToken
    {
        public PostfixUnaryOperatorToken(PostfixUnaryOperatorKind unaryOperator, byte precedence)
            : base(precedence)
        {
            Operator = unaryOperator;
        }

        public PostfixUnaryOperatorKind Operator { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.PostfixUnaryOperator;
    }

    internal sealed class BinaryOperatorToken : ExpressionToken
    {
        public BinaryOperatorToken(BinaryOperatorKind binaryOperator, byte precedence)
            : base(precedence)
        {
            Operator = binaryOperator;
        }

        public BinaryOperatorKind Operator { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.BinaryOperator;
    }

    internal sealed class AssignmentOperatorToken : ExpressionToken
    {
        public AssignmentOperatorToken(AssignmentOperatorKind assignmentOperator, byte precedence)
            : base(precedence)
        {
            Operator = assignmentOperator;
        }

        public AssignmentOperatorKind Operator { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.AssignmentOperator;
    }

    internal sealed class SpecialOperatorToken : ExpressionToken
    {
        public SpecialOperatorToken(SpecialOperatorKind specialOperator, byte precedence)
            : base(precedence)
        {
            Operator = specialOperator;
        }

        public SpecialOperatorKind Operator { get; }
        public override ExpressionTokenKind Kind => ExpressionTokenKind.SpecialOperator;
    }

    internal sealed class EndOfExpressionToken : ExpressionToken
    {
        public EndOfExpressionToken()
            : base(0)
        {
        }

        public override int Length { get; } = 1;
        public override ExpressionTokenKind Kind => ExpressionTokenKind.EndOfExpression;
    }
}

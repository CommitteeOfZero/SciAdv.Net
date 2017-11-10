using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SciAdvNet.SC3Script
{
    public abstract class Expression : Operand, IEquatable<Expression>
    {
        public ImmutableArray<byte> Bytes { get; internal set; }

        public bool Equals(Expression other) => ReferenceEquals(this, other) || Bytes.SequenceEqual(other?.Bytes);
        public override bool Equals(object obj) => Equals(obj as Expression);

        public override int GetHashCode()
        {
            return 1182642244 + EqualityComparer<ImmutableArray<byte>>.Default.GetHashCode(Bytes);
        }
    }

    public sealed class ConstantExpression : Expression
    {
        internal ConstantExpression(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitConstantExpression(this);
        }
    }

    public sealed class VariableExpression : Expression
    {
        internal VariableExpression(VariableKind kind, Expression variableId)
        {
            VariableKind = kind;
            VariableId = variableId;
        }

        public VariableKind VariableKind { get; }
        public Expression VariableId { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitVariableExpression(this);
        }
    }

    public sealed class PrefixUnaryExpression : Expression
    {
        internal PrefixUnaryExpression(PrefixUnaryOperatorKind prefixOperator, Expression operand)
        {
            Operator = prefixOperator;
            Operand = operand;
        }

        public PrefixUnaryOperatorKind Operator { get; }
        public Expression Operand { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitPrefixUnaryExpression(this);
        }
    }

    public sealed class PostfixUnaryExpression : Expression
    {
        internal PostfixUnaryExpression(Expression operand, PostfixUnaryOperatorKind postfixOperator)
        {
            Operand = operand;
            Operator = postfixOperator;
        }

        public Expression Operand { get; }
        public PostfixUnaryOperatorKind Operator { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitPostfixUnaryExpression(this);
        }
    }

    public sealed class BinaryExpression : Expression
    {
        internal BinaryExpression(Expression left, BinaryOperatorKind binaryOperator, Expression right)
        {
            Left = left;
            Operator = binaryOperator;
            Right = right;
        }

        public Expression Left { get; }
        public BinaryOperatorKind Operator { get; }
        public Expression Right { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitBinaryExpression(this);
        }
    }

    public sealed class AssignmentExpression : Expression
    {
        internal AssignmentExpression(Expression left, AssignmentOperatorKind assignmentOperator, Expression right)
        {
            Left = left;
            Operator = assignmentOperator;
            Right = right;
        }

        public Expression Left { get; }
        public AssignmentOperatorKind Operator { get; }
        public Expression Right { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitAssignmentExpression(this);
        }
    }

    public sealed class DataBlockReferenceExpression : Expression
    {
        internal DataBlockReferenceExpression(Expression blockId)
        {
            BlockId = blockId;
        }

        public Expression BlockId { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitDataBlockReferenceExpression(this);
        }
    }

    public sealed class DataBlockAccessExpression : Expression
    {
        internal DataBlockAccessExpression(DataBlockReferenceExpression blockReference, Expression elementIndex)
        {
            BlockReference = blockReference;
            ElementIndex = elementIndex;
        }

        public DataBlockReferenceExpression BlockReference { get; }
        public Expression ElementIndex { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitDataBlockAccessExpression(this);
        }
    }

    public sealed class RandomNumberExpression : Expression
    {
        internal RandomNumberExpression(Expression maxValue)
        {
            MaximumValue = maxValue;
        }

        public Expression MaximumValue { get; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitRandomNumberExpression(this);
        }
    }

    public enum VariableKind : byte
    {
        Global = 0x28,
        Flag = 0x29,
        ThreadLocal = 0x2D
    }
}

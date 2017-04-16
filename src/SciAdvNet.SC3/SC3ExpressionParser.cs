using SciAdvNet.Common;
using SciAdvNet.SC3.Utils;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3
{
    public static class SC3ExpressionParser
    {
        private const byte ConstantLengthMask = 0xE0;
        private const byte ConstantSignMask = 0x10;
        private const byte PositiveConstantValueMask = 0x1F;
        private const byte NegativeConstantValueMask = 0xE0;

        public static Expression ParseExpression(BinaryReader reader)
        {
            long posStart = reader.BaseStream.Position;
            var result = ParseSubExpression(reader, 0);
            long posEnd = reader.BaseStream.Position;

            reader.BaseStream.Position = posStart;
            var bytes = reader.ReadBytes((int)(posEnd - posStart));
            result.Bytes = bytes.ToImmutableArray();

            return result;
        }

        public static Expression ParseExpression(byte[] expression)
        {
            using (var stream = new MemoryStream(expression))
            using (var reader = new BinaryReader(stream))
            {
                return ParseExpression(reader);
            }
        }

        private static Expression ParseSubExpression(BinaryReader reader, byte minPrecedence)
        {
            var leftOperand = ParseTerm(reader);

            ExpressionToken token;
            while ((token = PeekExpressionToken(reader)).Precedence >= minPrecedence)
            {
                EatExpressionToken(reader, token);
                if (token.Kind == ExpressionTokenKind.BinaryOperator || token.Kind == ExpressionTokenKind.AssignmentOperator)
                {
                    byte newPrecedence = (byte)(token.Precedence + 1);
                    var rightOperand = ParseSubExpression(reader, newPrecedence);

                    if (token.Kind == ExpressionTokenKind.BinaryOperator)
                    {
                        var opToken = token as BinaryOperatorToken;
                        leftOperand = Expression.Binary(leftOperand, opToken.Operator, rightOperand);
                    }
                    else
                    {
                        var opToken = token as AssignmentOperatorToken;
                        leftOperand = Expression.Assignment(leftOperand, opToken.Operator, rightOperand);
                    }
                }
                else
                {
                    break;
                }
            }

            return leftOperand;
        }

        private static Expression ParseTerm(BinaryReader reader)
        {
            var token = NextExpressionToken(reader);

            Expression term = null;
            if (token.Kind == ExpressionTokenKind.Constant)
            {
                var constToken = token as ConstantValueToken;
                term = Expression.Constant(constToken.Value);
            }
            else if (token.Kind == ExpressionTokenKind.PrefixUnaryOperator)
            {
                var opToken = token as PrefixUnaryOperatorToken;
                var operand = ParseSubExpression(reader, opToken.Precedence);
                term = Expression.PrefixUnary(opToken.Operator, operand);
            }
            else if (token.Kind == ExpressionTokenKind.SpecialOperator)
            {
                var specialOp = token as SpecialOperatorToken;
                byte newPrecedence = (byte)(specialOp.Precedence + 1);
                var operand = ParseSubExpression(reader, newPrecedence);
                switch (specialOp.Operator)
                {
                    case SpecialOperatorKind.GlobalVar:
                    case SpecialOperatorKind.Flag:
                    case SpecialOperatorKind.ThreadLocalVar:
                        term = Expression.Variable((VariableKind)specialOp.Operator, operand);
                        break;

                    case SpecialOperatorKind.DataBlockReference:
                        term = Expression.DataBlockReference(operand);
                        break;

                    case SpecialOperatorKind.DataBlockAccess:
                        var elementIndex = ParseSubExpression(reader, 0);
                        term = Expression.ArrayAccess(operand as DataBlockReferenceExpression, elementIndex);
                        break;

                    case SpecialOperatorKind.RandomNumber:
                        term = Expression.RandomNumber(operand);
                        break;
                }
            }

            return ParsePostfixExpression(reader, term);
        }

        private static Expression ParsePostfixExpression(BinaryReader reader, Expression expr)
        {
            var peek = PeekExpressionToken(reader);
            if (peek.Kind == ExpressionTokenKind.PostfixUnaryOperator)
            {
                EatExpressionToken(reader, peek);
                var opToken = peek as PostfixUnaryOperatorToken;
                expr = Expression.PostfixUnary(expr, opToken.Operator);
            }

            return expr;
        }

        private static ExpressionToken NextExpressionToken(BinaryReader reader)
        {
            int originalPosition = (int)reader.BaseStream.Position;
            byte peek = reader.PeekByte();
            if (peek == 0x00)
            {
                reader.ReadByte();
                return new EndOfExpressionToken();
            }

            byte precedence;
            if (peek >= 0x80)
            {
                int value = ReadConstant(reader);
                precedence = reader.ReadByte();
                int length = (int)reader.BaseStream.Position - originalPosition;
                return new ConstantValueToken(value, precedence, length);
            }

            byte code = reader.ReadByte();
            precedence = reader.ReadByte();

            if (IsPrefixUnary(code))
            {
                return new PrefixUnaryOperatorToken((PrefixUnaryOperatorKind)code, precedence);
            }
            if (IsPostfixUnary(code))
            {
                return new PostfixUnaryOperatorToken((PostfixUnaryOperatorKind)code, precedence);
            }
            if (IsBinary(code))
            {
                return new BinaryOperatorToken((BinaryOperatorKind)code, precedence);
            }
            if (IsAssignment(code))
            {
                return new AssignmentOperatorToken((AssignmentOperatorKind)code, precedence);
            }
            if (IsSpecialOperator(code))
            {
                return new SpecialOperatorToken((SpecialOperatorKind)code, precedence);
            }

            throw new InvalidDataException($"0x{code:X2}{precedence:X2} is not a valid expression token.");
        }

        private static ExpressionToken PeekExpressionToken(BinaryReader reader)
        {
            var token = NextExpressionToken(reader);
            reader.BaseStream.Position -= token.Length;
            return token;
        }

        private static void EatExpressionToken(BinaryReader reader, ExpressionToken token)
        {
            reader.BaseStream.Position += token.Length;
        }

        private static bool IsPrefixUnary(byte op)
        {
            return (PrefixUnaryOperatorKind)op == PrefixUnaryOperatorKind.BitwiseNegation;
        }

        private static bool IsPostfixUnary(byte op)
        {
            switch ((PostfixUnaryOperatorKind)op)
            {
                case PostfixUnaryOperatorKind.PostfixDecrement:
                case PostfixUnaryOperatorKind.PostfixIncrement:
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsBinary(byte op)
        {
            switch ((BinaryOperatorKind)op)
            {
                case BinaryOperatorKind.Addition:
                case BinaryOperatorKind.BitwiseAnd:
                case BinaryOperatorKind.BitwiseOr:
                case BinaryOperatorKind.BitwiseXor:
                case BinaryOperatorKind.Division:
                case BinaryOperatorKind.Equal:
                case BinaryOperatorKind.GreaterThan:
                case BinaryOperatorKind.GreaterThanOrEqual:
                case BinaryOperatorKind.LeftShift:
                case BinaryOperatorKind.LessThan:
                case BinaryOperatorKind.LessThanOrEqual:
                case BinaryOperatorKind.Modulo:
                case BinaryOperatorKind.Multiplication:
                case BinaryOperatorKind.NotEqual:
                case BinaryOperatorKind.RightShift:
                case BinaryOperatorKind.Subtraction:
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsAssignment(byte op)
        {
            switch ((AssignmentOperatorKind)op)
            {
                case AssignmentOperatorKind.AddAssignment:
                case AssignmentOperatorKind.AndAssignment:
                case AssignmentOperatorKind.DivideAssignment:
                case AssignmentOperatorKind.ExclusiveOrAssignment:
                case AssignmentOperatorKind.LeftShiftAssignment:
                case AssignmentOperatorKind.ModuloAssignment:
                case AssignmentOperatorKind.MultiplyAssignment:
                case AssignmentOperatorKind.OrAssignment:
                case AssignmentOperatorKind.RightShiftAssignment:
                case AssignmentOperatorKind.SimpleAssignment:
                case AssignmentOperatorKind.SubtractAssignment:
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsSpecialOperator(byte op)
        {
            switch ((SpecialOperatorKind)op)
            {
                case SpecialOperatorKind.DataBlockAccess:
                case SpecialOperatorKind.DataBlockReference:
                case SpecialOperatorKind.Flag:
                case SpecialOperatorKind.GlobalVar:
                case SpecialOperatorKind.RandomNumber:
                case SpecialOperatorKind.ThreadLocalVar:
                    return true;

                default:
                    return false;
            }
        }

        private static int ReadConstant(BinaryReader reader)
        {
            byte firstByte = reader.ReadByte();
            int length = GetConstantLength(firstByte);
            if (length == 4)
            {
                return reader.ReadInt32BE();
            }

            bool positive = IsPositiveConstant(firstByte);
            firstByte = positive ? firstByte &= PositiveConstantValueMask : firstByte |= NegativeConstantValueMask;

            // TODO: make this more clear, get rid of the LINQ magic.
            var constBytes = new[] { firstByte }.Concat(reader.ReadBytes(length - 1).Reverse()).ToArray();
            constBytes = Enumerable.Repeat((byte)(positive ? 0x00 : 0xFF), 4 - constBytes.Length).Concat(constBytes).ToArray();

            return BinaryUtils.BytesToInt32(constBytes, ByteOrder.BigEndian);
        }

        private static int GetConstantLength(byte constFirstByte)
        {
            return (constFirstByte & ConstantLengthMask - 0x80) / 0x20 + 1;
        }

        private static bool IsPositiveConstant(byte constFirstByte)
        {
            return (constFirstByte & ConstantSignMask) == 0;
        }
    }
}

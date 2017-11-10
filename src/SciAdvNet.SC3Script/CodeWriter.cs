using System.Collections.Generic;
using System.IO;

namespace SciAdvNet.SC3Script
{
    public abstract class CodeWriter : CodeVisitor
    {
        private static readonly Dictionary<PrefixUnaryOperatorKind, string> PrefixUnaryOperators;
        private static readonly Dictionary<PostfixUnaryOperatorKind, string> PostfixUnaryOperators;
        private static readonly Dictionary<BinaryOperatorKind, string> BinaryOperators;
        private static readonly Dictionary<AssignmentOperatorKind, string> AssignmentOperators;

        static CodeWriter()
        {
            PrefixUnaryOperators = new Dictionary<PrefixUnaryOperatorKind, string>()
            {
                [PrefixUnaryOperatorKind.BitwiseNegation] = "~"
            };

            PostfixUnaryOperators = new Dictionary<PostfixUnaryOperatorKind, string>()
            {
                [PostfixUnaryOperatorKind.PostfixIncrement] = "++",
                [PostfixUnaryOperatorKind.PostfixDecrement] = "--"
            };

            BinaryOperators = new Dictionary<BinaryOperatorKind, string>()
            {
                [BinaryOperatorKind.Addition] = "+",
                [BinaryOperatorKind.Subtraction] = "-",
                [BinaryOperatorKind.Multiplication] = "*",
                [BinaryOperatorKind.Division] = "/",
                [BinaryOperatorKind.Modulo] = "%",
                [BinaryOperatorKind.LeftShift] = "<<",
                [BinaryOperatorKind.RightShift] = ">>",
                [BinaryOperatorKind.BitwiseAnd] = "&",
                [BinaryOperatorKind.BitwiseOr] = "|",
                [BinaryOperatorKind.BitwiseXor] = "^",
                [BinaryOperatorKind.Equal] = "==",
                [BinaryOperatorKind.NotEqual] = "!=",
                [BinaryOperatorKind.GreaterThan] = ">",
                [BinaryOperatorKind.GreaterThanOrEqual] = ">=",
                [BinaryOperatorKind.LessThan] = "<",
                [BinaryOperatorKind.LessThanOrEqual] = "<=",
            };

            AssignmentOperators = new Dictionary<AssignmentOperatorKind, string>()
            {
                [AssignmentOperatorKind.SimpleAssignment] = "=",
                [AssignmentOperatorKind.AddAssignment] = "+=",
                [AssignmentOperatorKind.SubtractAssignment] = "-=",
                [AssignmentOperatorKind.MultiplyAssignment] = "*=",
                [AssignmentOperatorKind.DivideAssignment]  = "/=",
                [AssignmentOperatorKind.ModuloAssignment] = "%=",
                [AssignmentOperatorKind.LeftShiftAssignment] = "<<=",
                [AssignmentOperatorKind.RightShiftAssignment] = ">>=",
                [AssignmentOperatorKind.AndAssignment] = "&=",
                [AssignmentOperatorKind.OrAssignment] = "|=",
                [AssignmentOperatorKind.ExclusiveOrAssignment] = "^="
            };
        }

        private readonly TextWriter _writer;
        private bool _writeIndent;
        private int _indent;

        protected CodeWriter(TextWriter textWriter)
        {
            _writer = textWriter;
        }

        public void Write(string str)
        {
            if (_writeIndent)
            {
                WriteIndent();
            }

            _writer.Write(str);
            _writeIndent = false;
        }

        public void WriteSpace()
        {
            _writer.Write(" ");
        }

        public void WriteLine()
        {
            _writer.WriteLine();
            _writeIndent = true;
        }

        public void WritePrefixUnaryOperator(PrefixUnaryOperatorKind op)
        {
            Write(PrefixUnaryOperators[op]);
        }

        public void WritePostfixInaryOperator(PostfixUnaryOperatorKind op)
        {
            Write(PostfixUnaryOperators[op]);
        }

        public void WriteBinaryOperator(BinaryOperatorKind op)
        {
            Write(BinaryOperators[op]);
        }

        public void WriteAssignmentOperator(AssignmentOperatorKind op)
        {
            Write(AssignmentOperators[op]);
        }

        public void Indent()
        {
            _indent++;
        }

        public void Outdent()
        {
            _indent--;
        }

        private void WriteIndent()
        {
            for (int i = 0; i < _indent; i++)
            {
                _writer.Write("\t");
            }
        }

        public abstract void WriteCodeBlock(DisassemblyResult codeBlock);
    }
}

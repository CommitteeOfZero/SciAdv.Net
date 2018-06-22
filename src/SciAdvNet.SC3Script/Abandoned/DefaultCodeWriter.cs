using SciAdvNet.SC3Script.Utils;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3Script
{
    public class DefaultCodeWriter : CodeWriter
    {
        public DefaultCodeWriter(TextWriter textWriter)
            : base(textWriter)
        {
        }

        public override void WriteCodeBlock(DisassemblyResult codeBlock)
        {
            Visit(codeBlock);
        }

        public override void VisitCodeBlock(DisassemblyResult codeBlock)
        {
            Write($"#label_{codeBlock.CodeBlock.Id}:");
            WriteLine();
            Indent();

            foreach (var instruction in codeBlock.Instructions)
            {
                Visit(instruction);
            }

            if (codeBlock.UnrecognizedBytes.Length > 0)
            {
                var bytes = codeBlock.UnrecognizedBytes.ToArray();
                string unrecognizedPart = BinaryUtils.BytesToHexString(bytes, " ");

                WriteLine();
                Write("#unrecognized:");
                WriteLine();
                Write(unrecognizedPart);
            }

            Outdent();
            WriteLine();
        }

        public override void DefaultVisitInstruction(Instruction instruction)
        {
            Write(instruction.Name);
            if (instruction.Operands.Length > 0)
            {
                Write("(");
            }

            var operands = instruction.Operands;
            for (int i = 0; i < operands.Length; i++)
            {
                var currOperand = operands[i];
                if (i > 0)
                {
                    Write(",");
                    WriteSpace();
                }

                Write(currOperand.Name);
                Write(":");
                WriteSpace();
                Visit(currOperand);
            }

            if (instruction.Operands.Length > 0)
            {
                Write(")");
            }

            WriteLine();
        }

        public override void VisitAssignInstruction(AssignInstruction instruction)
        {
            Visit(instruction.Operands.First());
            WriteLine();
        }

        public override void VisitShowDialogueWindowInstruction(ShowDialogueWindowInstruction instruction)
        {
            var sayIntr = instruction.Next?.Next?.Next as LoadVoiceActedDialogueLineInstruction;
            if (sayIntr != null)
            {

            }
            else
            {
                base.VisitShowDialogueWindowInstruction(instruction);
            }
        }

        public override void VisitPrimitiveTypeValue(PrimitiveTypeValue contantValue)
        {
            Write(contantValue.Value.ToString());
        }

        public override void VisitConstantExpression(ConstantExpression expression)
        {
            Write(expression.Value.ToString());
        }

        public override void VisitVariableExpression(VariableExpression expression)
        {
            switch (expression.VariableKind)
            {
                case VariableKind.Global:
                    Write("GlobalVars");
                    break;

                case VariableKind.ThreadLocal:
                    Write("ThreadVars");
                    break;

                case VariableKind.Flag:
                    Write("Flags");
                    break;
            }

            Write("[");
            Visit(expression.VariableId);
            Write("]");
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpression expression)
        {
            WritePrefixUnaryOperator(expression.Operator);
            Visit(expression.Operand);
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpression expression)
        {
            WritePostfixInaryOperator(expression.Operator);
            Visit(expression.Operand);
        }

        public override void VisitBinaryExpression(BinaryExpression expression)
        {
            Visit(expression.Left);
            WriteSpace();
            WriteBinaryOperator(expression.Operator);
            WriteSpace();
            Visit(expression.Right);
        }

        public override void VisitAssignmentExpression(AssignmentExpression expression)
        {
            Visit(expression.Left);
            WriteSpace();
            WriteAssignmentOperator(expression.Operator);
            WriteSpace();
            Visit(expression.Right);
        }

        public override void VisitDataBlockReferenceExpression(DataBlockReferenceExpression expression)
        {
            Write("DataBlocks");
            Write("[");
            Visit(expression.BlockId);
            Write("]");
        }

        public override void VisitDataBlockAccessExpression(DataBlockAccessExpression expression)
        {
            Write("DataAccess(");
            Visit(expression.BlockReference);
            Write(", ");
            Visit(expression.ElementIndex);
            Write(")");
        }

        public override void VisitRandomNumberExpression(RandomNumberExpression expression)
        {
            Write("RandomNumber");
            Write("(");
            Visit(expression.MaximumValue);
            Write(")");
        }
    }
}

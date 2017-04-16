namespace SciAdvNet.SC3
{
    public class CodeWalker : CodeVisitor
    {
        public override void VisitCodeBlock(DecompilationResult codeBlock)
        {
            foreach (var instruction in codeBlock.Instructions)
            {
                Visit(instruction);
            }
        }

        public override void DefaultVisitInstruction(Instruction instruction)
        {
            foreach (var operand in instruction.Operands)
            {
                Visit(operand);
            }
        }

        public override void VisitVariableExpression(VariableExpression expression)
        {
            Visit(expression.VariableId);
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpression expression)
        {
            Visit(expression.Operand);
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpression expression)
        {
            Visit(expression.Operand);
        }

        public override void VisitBinaryExpression(BinaryExpression expression)
        {
            Visit(expression.Left);
            Visit(expression.Right);
        }

        public override void VisitAssignmentExpression(AssignmentExpression expression)
        {
            Visit(expression.Left);
            Visit(expression.Right);
        }

        public override void VisitDataBlockReferenceExpression(DataBlockReferenceExpression expression)
        {
            Visit(expression.BlockId);
        }

        public override void VisitDataBlockAccessExpression(DataBlockAccessExpression expression)
        {
            Visit(expression.BlockReference);
            Visit(expression.ElementIndex);
        }

        public override void VisitRandomNumberExpression(RandomNumberExpression expression)
        {
            Visit(expression.MaximumValue);
        }
    }
}

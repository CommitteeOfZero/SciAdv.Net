using System;
using System.Collections.Generic;

namespace SciAdvNet.NSScript
{
    public class SyntaxVisitor
    {
        protected void Visit(SyntaxNode node)
        {
            node.Accept(this);
        }

        private void DefaultVisitNode(SyntaxNode node) { }

        protected void VisitArray(IEnumerable<SyntaxNode> list)
        {
            foreach (var node in list)
            {
                Visit(node);
            }
        }

        internal virtual void VisitChapter(Chapter chapter)
        {
            DefaultVisitNode(chapter);
        }

        internal virtual void VisitMethod(Method method)
        {
            DefaultVisitNode(method);
        }

        internal virtual void VisitBlock(Block block)
        {
            DefaultVisitNode(block);
        }

        internal virtual void VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            DefaultVisitNode(expressionStatement);
        }

        internal virtual void VisitLiteral(Literal literal)
        {
            DefaultVisitNode(literal);
        }

        internal virtual void VisitConstantValue(ConstantValue constantValue)
        {
            DefaultVisitNode(constantValue);
        }

        internal virtual void VisitIdentifier(Identifier identifier)
        {
            DefaultVisitNode(identifier);
        }

        internal virtual void VisitNamedConstant(NamedConstant namedConstant)
        {
            DefaultVisitNode(namedConstant);
        }

        internal virtual void VisitVariable(Variable variable)
        {
            DefaultVisitNode(variable);
        }

        internal virtual void VisitParameterReference(ParameterReference parameterReference)
        {
            DefaultVisitNode(parameterReference);
        }

        internal virtual void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            DefaultVisitNode(unaryExpression);
        }

        internal virtual void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            DefaultVisitNode(binaryExpression);
        }

        internal virtual void VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            DefaultVisitNode(assignmentExpression);
        }

        internal virtual void VisitMethodCall(MethodCall methodCall)
        {
            DefaultVisitNode(methodCall);
        }

        internal virtual void VisitIfStatement(IfStatement ifStatement)
        {
            DefaultVisitNode(ifStatement);
        }

        internal virtual void VisitWhileStatement(WhileStatement whileStatement)
        {
            DefaultVisitNode(whileStatement);
        }

        internal virtual void VisitReturnStatement(ReturnStatement returnStatement)
        {
            DefaultVisitNode(returnStatement);
        }

        internal virtual void VisitSelectStatement(SelectStatement selectStatement)
        {
            DefaultVisitNode(selectStatement);
        }

        internal void VisitScene(Scene scene)
        {
            throw new NotImplementedException();
        }

        internal virtual void VisitSelectSection(SelectSection selectSection)
        {
            DefaultVisitNode(selectSection);
        }

        internal virtual void VisitCallChapterStatement(CallChapterStatement callChapterStatement)
        {
            DefaultVisitNode(callChapterStatement);
        }

        internal virtual void VisitCallSceneStatement(CallSceneStatement callSceneStatement)
        {
            DefaultVisitNode(callSceneStatement);
        }

        internal virtual void VisitDialogueBlock(DialogueBlock dialogueBlock)
        {
            DefaultVisitNode(dialogueBlock);
        }

        internal virtual void VisitVoice(Voice voice)
        {
            DefaultVisitNode(voice);
        }

        internal virtual void VisitDialogueLine(DialogueLine dialogueLine)
        {
            DefaultVisitNode(dialogueLine);
        }

        internal virtual void VisitTextSegment(TextSegment textSegment)
        {
            DefaultVisitNode(textSegment);
        }
    }

    public class SyntaxVisitor<TResult>
    {
        protected TResult Visit(SyntaxNode node)
        {
            if (node == null)
            {
                return default(TResult);
            }

            return node.Accept(this);
        }

        protected void VisitArray(IEnumerable<SyntaxNode> list)
        {
            foreach (var node in list)
            {
                Visit(node);
            }
        }

        private TResult DefaultVisitNode(SyntaxNode node) => default(TResult);

        internal virtual TResult VisitChapter(Chapter chapter)
        {
            return DefaultVisitNode(chapter);
        }

        internal virtual TResult VisitMethod(Method method)
        {
            return DefaultVisitNode(method);
        }

        internal virtual TResult VisitBlock(Block block)
        {
            return DefaultVisitNode(block);
        }

        internal virtual TResult VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            return DefaultVisitNode(expressionStatement);
        }

        internal virtual TResult VisitLiteral(Literal literal)
        {
            return DefaultVisitNode(literal);
        }

        internal virtual TResult VisitConstantValue(ConstantValue constantValue)
        {
            return DefaultVisitNode(constantValue);
        }

        internal virtual TResult VisitIdentifier(Identifier identifier)
        {
            return DefaultVisitNode(identifier);
        }

        internal virtual TResult VisitNamedConstant(NamedConstant namedConstant)
        {
            return DefaultVisitNode(namedConstant);
        }

        internal virtual TResult VisitVariable(Variable variable)
        {
            return DefaultVisitNode(variable);
        }

        internal virtual TResult VisitParameterReference(ParameterReference parameterReference)
        {
            return DefaultVisitNode(parameterReference);
        }

        internal virtual TResult VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            return DefaultVisitNode(unaryExpression);
        }

        internal virtual TResult VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            return DefaultVisitNode(binaryExpression);
        }

        internal virtual TResult VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            return DefaultVisitNode(assignmentExpression);
        }

        internal virtual TResult VisitMethodCall(MethodCall methodCall)
        {
            return DefaultVisitNode(methodCall);
        }

        internal virtual TResult VisitIfStatement(IfStatement ifStatement)
        {
            return DefaultVisitNode(ifStatement);
        }

        internal virtual TResult VisitWhileStatement(WhileStatement whileStatement)
        {
            return DefaultVisitNode(whileStatement);
        }

        internal virtual TResult VisitReturnStatement(ReturnStatement returnStatement)
        {
            return DefaultVisitNode(returnStatement);
        }

        internal virtual TResult VisitSelectStatement(SelectStatement selectStatement)
        {
            return DefaultVisitNode(selectStatement);
        }

        internal virtual TResult VisitSelectSection(SelectSection selectSection)
        {
            return DefaultVisitNode(selectSection);
        }

        internal TResult VisitCallSceneStatement(CallSceneStatement callSceneStatement)
        {
            return DefaultVisitNode(callSceneStatement);
        }

        internal TResult VisitCallChapterStatement(CallChapterStatement callChapterStatement)
        {
            return DefaultVisitNode(callChapterStatement);
        }

        internal virtual TResult VisitDialogueBlock(DialogueBlock dialogueBlock)
        {
            return DefaultVisitNode(dialogueBlock);
        }

        internal virtual TResult VisitVoice(Voice voice)
        {
            return DefaultVisitNode(voice);
        }

        internal virtual TResult VisitDialogueLine(DialogueLine dialogueLine)
        {
            return DefaultVisitNode(dialogueLine);
        }

        internal virtual TResult VisitTextSegment(TextSegment textSegment)
        {
            return DefaultVisitNode(textSegment);
        }

        internal TResult VisitScene(Scene scene)
        {
            throw new NotImplementedException();
        }
    }
}

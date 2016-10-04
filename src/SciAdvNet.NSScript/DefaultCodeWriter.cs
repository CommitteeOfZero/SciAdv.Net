﻿using System.IO;

namespace SciAdvNet.NSScript
{
    public class DefaultCodeWriter : CodeWriter
    {
        public DefaultCodeWriter(TextWriter textWriter)
            : base(textWriter)
        {
        }

        internal override void VisitChapter(Chapter chapter)
        {
            Write(SyntaxFacts.GetText(SyntaxTokenKind.ChapterKeyword));
            WriteSpace();
            Visit(chapter.Name);
            Visit(chapter.Body);
        }

        internal override void VisitMethod(Method method)
        {
            Write(SyntaxFacts.GetText(SyntaxTokenKind.FunctionKeyword));
            WriteSpace();
            Visit(method.Name);
            Write("(");

            var parameters = method.Parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                Visit(method.Parameters[i]);
                if (i != parameters.Length - 1)
                {
                    Write(", ");
                }
            }

            Write(")");
            Visit(method.Body);
        }

        internal override void VisitBlock(Block block)
        {
            WriteLine();
            Write("{");
            WriteLine();
            Indent();

            foreach (var statement in block.Statements)
            {
                Visit(statement);
                WriteLine();
            }

            Outdent();
            Write("}");
            WriteLine();
        }

        internal override void VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            Visit(expressionStatement.Expression);
            Write(";");
        }

        internal override void VisitLiteral(Literal literal)
        {
            Write(literal.Text);
        }

        internal override void VisitIdentifier(Identifier identifier)
        {
            Write(identifier.FullName);
        }

        internal override void VisitNamedConstant(NamedConstant constant)
        {
            Visit(constant.Name);
        }

        internal override void VisitVariable(Variable variable)
        {
            Visit(variable.Name);
        }

        internal override void VisitParameterReference(ParameterReference parameter)
        {
            Visit(parameter.ParameterName);
        }

        internal override void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            var opKind = unaryExpression.OperationKind;
            if (Operation.IsPrefixOperation(opKind))
            {
                Write(Operation.GetText(opKind));
            }

            Visit(unaryExpression.Operand);

            if (Operation.IsPostfixOperation(opKind))
            {
                Write(Operation.GetText(opKind));
            }
        }

        internal override void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            Visit(binaryExpression.Left);
            WriteSpace();
            Write(Operation.GetText(binaryExpression.OperationKind));
            WriteSpace();
            Visit(binaryExpression.Right);
        }

        internal override void VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            Visit(assignmentExpression.Target);
            WriteSpace();
            Write(Operation.GetText(assignmentExpression.OperationKind));
            WriteSpace();
            Visit(assignmentExpression.Value);
        }

        internal override void VisitMethodCall(MethodCall methodCall)
        {
            Visit(methodCall.TargetMethodName);
            Write("(");

            var args = methodCall.Arguments;
            for (int i = 0; i < args.Length; i++)
            {
                Visit(methodCall.Arguments[i]);
                if (i != args.Length - 1)
                {
                    Write(", ");
                }
            }

            Write(");");
        }

        internal override void VisitIfStatement(IfStatement ifStatement)
        {
            Write(SyntaxFacts.GetText(SyntaxTokenKind.IfKeyword));
            WriteSpace();
            Write("(");
            Visit(ifStatement.Condition);
            Write(")");

            bool block = ifStatement.IfTrueStatement.Kind == SyntaxNodeKind.Block;
            if (!block)
            {
                WriteLine();
                Indent();
                Visit(ifStatement.IfTrueStatement);
                Outdent();
                WriteLine();
            }
            else
            {
                Visit(ifStatement.IfTrueStatement);
            }


            if (ifStatement.IfFalseStatement != null)
            {
                Write(SyntaxFacts.GetText(SyntaxTokenKind.ElseKeyword));

                block = ifStatement.IfFalseStatement.Kind == SyntaxNodeKind.Block;
                bool elif = ifStatement.IfFalseStatement.Kind == SyntaxNodeKind.IfStatement;
                if (!block && !elif)
                {
                    WriteLine();
                    Indent();
                    Visit(ifStatement.IfTrueStatement);
                    Outdent();
                    WriteLine();
                }
                else if (elif)
                {
                    WriteSpace();
                }

                Visit(ifStatement.IfFalseStatement);
            }
        }

        internal override void VisitWhileStatement(WhileStatement whileStatement)
        {
            Write(SyntaxFacts.GetText(SyntaxTokenKind.WhileKeyword));
            WriteSpace();
            Write("(");
            Visit(whileStatement.Condition);
            Write(")");

            if (whileStatement.Body.Kind != SyntaxNodeKind.Block)
            {
                WriteLine();
                Indent();
                Visit(whileStatement.Body);
                Outdent();
                WriteLine();
            }
            else
            {
                Visit(whileStatement.Body);
            }
        }

        internal override void VisitDialogueBlock(DialogueBlock dialogueBlock)
        {
            Write($"<PRE {dialogueBlock.BoxName}>");
            WriteLine();
            Write($"[{dialogueBlock.Identifier}]");
            WriteLine();
            Write("</PRE>");
        }
    }
}

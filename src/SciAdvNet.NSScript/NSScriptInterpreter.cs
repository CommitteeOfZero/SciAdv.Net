using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SciAdvNet.NSScript
{
    public class NSScriptInterpreter : SyntaxVisitor<Expression>
    {
        private readonly IExecutionEnvironment _env;
        private readonly INssExports _exports;
        private readonly NSScriptSession _session;

        private ImmutableArray<ParameterReference> _scope;

        public NSScriptInterpreter(IExecutionEnvironment executionEnvironment, INssExports exports)
        {
            _env = executionEnvironment;
            _exports = exports;
            _session = new NSScriptSession(executionEnvironment);
        }

        public NSScript CurrentModule { get; private set; }

        public void ExecuteModule(string moduleName)
        {
            CurrentModule = _session.GetModule(moduleName);
        }

        private ConstantValue Evaluate(Expression expression) => Visit(expression) as ConstantValue;
        private Expression Execute(Statement statement) => Visit(statement);
        private Expression Execute(IEnumerable<Statement> statements)
        {
            VisitArray(statements);
            return default(Expression);
        }

        private void GoTo(string methodName)
        {
            var method = LocateMethod(methodName);
            if (method == null)
            {
                throw new ArgumentException($"Couldn't locate method '{methodName}'.", nameof(methodName));
            }
        }

        private Method LocateMethod(string name)
        {
            return CurrentModule.Methods
                .SingleOrDefault(x => x.Name.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private void Call(Method method, ImmutableArray<ConstantValue> arguments)
        {

        }


        internal override Expression VisitChapter(Chapter chapter)
        {
            return Execute(chapter.Body);
        }

        internal override Expression VisitMethod(Method method)
        {
            _scope = method.Parameters;
            return Execute(method.Body);
        }

        internal override Expression VisitBlock(Block block)
        {
            return Execute(block.Statements);
        }

        internal override Expression VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            return Evaluate(expressionStatement.Expression);
        }

        internal override Expression VisitLiteral(Literal literal)
        {
            return literal.Value;
        }

        internal override Expression VisitVariable(Variable variable)
        {
            return _env.GetVariable(variable.Name.FullName);
        }

        internal override Expression VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            try
            {
                return ApplyUnaryOperation(unaryExpression.Operand, unaryExpression.OperationKind);
            }
            catch (InvalidOperationException e)
            {
                throw ExceptionUtilities.RuntimeError(CurrentModule.Name, e.Message, e);
            }
        }

        internal override Expression VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            string targetName = assignmentExpression.Target.Name.FullName;
            var value = Evaluate(assignmentExpression.Value);
            _env.SetVariable(targetName, value);
            return value;
        }

        internal override Expression VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            var leftValue = Evaluate(binaryExpression.Left);
            var rightValue = Evaluate(binaryExpression.Right);

            try
            {
                return ApplyBinaryOperation(leftValue, binaryExpression.OperationKind, rightValue);
            }
            catch (InvalidOperationException e)
            {
                throw ExceptionUtilities.RuntimeError(CurrentModule.Name, e.Message, e);
            }
        }

        private static ConstantValue ApplyBinaryOperation(ConstantValue leftOperand, BinaryOperationKind operationKind, ConstantValue rightOperand)
        {
            switch (operationKind)
            {
                case BinaryOperationKind.Addition:
                    return leftOperand + rightOperand;
                case BinaryOperationKind.Subtraction:
                    return leftOperand - rightOperand;
                case BinaryOperationKind.Multiplication:
                    return leftOperand * rightOperand;
                case BinaryOperationKind.Division:
                    return leftOperand / rightOperand;
                case BinaryOperationKind.Equal:
                    return leftOperand == rightOperand;
                case BinaryOperationKind.NotEqual:
                    return leftOperand != rightOperand;
                case BinaryOperationKind.LessThan:
                    return leftOperand < rightOperand;
                case BinaryOperationKind.LessThanOrEqual:
                    return leftOperand <= rightOperand;
                case BinaryOperationKind.GreaterThan:
                    return leftOperand > rightOperand;
                case BinaryOperationKind.GreaterThanOrEqual:
                    return leftOperand >= rightOperand;
                case BinaryOperationKind.LogicalAnd:
                    return leftOperand && rightOperand;
                case BinaryOperationKind.LogicalOr:
                default:
                    return leftOperand || rightOperand;
            }
        }

        private ConstantValue ApplyUnaryOperation(Expression operand, UnaryOperationKind operationKind)
        {
            if (operationKind == UnaryOperationKind.LogicalNegation)
            {
                return !Evaluate(operand);
            }

            if (operand.Kind != SyntaxNodeKind.Variable &&
                (operationKind == UnaryOperationKind.PostfixIncrement || operationKind == UnaryOperationKind.PostfixIncrement))
            {
                string op = Operation.GetText(operationKind);
                throw new InvalidOperationException($"Unary operator '{op}' can only be applied to variables.");
            }

            ConstantValue oldValue;

            string variableName = string.Empty;
            if (operand.Kind == SyntaxNodeKind.Variable)
            {
                variableName = (operand as Variable).Name.FullName;
                oldValue = _env?.GetVariable((operand as Variable).Name.FullName);
            }
            else
            {
                oldValue = Evaluate(operand);
            }

            switch (operationKind)
            {
                case UnaryOperationKind.UnaryPlus:
                    return oldValue;

                case UnaryOperationKind.UnaryMinus:
                    return -oldValue;

                case UnaryOperationKind.PostfixIncrement:
                    _env.SetVariable(variableName, oldValue++);
                    return oldValue;

                case UnaryOperationKind.PostfixDecrement:
                default:
                    _env.SetVariable(variableName, oldValue--);
                    return oldValue;
            }
        }

        internal override Expression VisitIfStatement(IfStatement ifStatement)
        {
            if (Evaluate(ifStatement.Condition))
            {
                Execute(ifStatement.IfTrueStatement);
            }
            else
            {
                Execute(ifStatement.IfFalseStatement);
            }

            return default(Expression);
        }

        internal override Expression VisitWhileStatement(WhileStatement whileStatement)
        {
            while (Evaluate(whileStatement.Condition))
            {
                Execute(whileStatement.Body);
            }

            return default(Expression);
        }

        internal override Expression VisitMethodCall(MethodCall methodCall)
        {
            var args = methodCall.Arguments.Select(x => Visit(x) as ConstantValue).ToImmutableArray();
            switch (methodCall.TargetMethodName.FullName)
            {
                case "SetAlias":
                    SetAlias(args);
                    break;

                case "Wait":
                    Wait(args);
                    break;

                case "WaitKey":
                    WaitKey(args);
                    break;

                case "LoadImage":
                    LoadImage(args);
                    break;

                case "CreateColor":
                    CreateColor(args);
                    break;

                case "CreateSound":
                    CreateSound(args);
                    break;

                case "CreateTexture":
                    CreateTexture(args);
                    break;

                default:
                    string methodName = methodCall.TargetMethodName.FullName;
                    Method method;
                    if ((method = LocateMethod(methodName)) != null)
                    {
                        Execute(method);
                    }
                    else
                    {
                        _exports.Unknown(methodName, args);
                    }
                    break;
            }

            return default(Expression);
        }

        private TResult Argument<TResult>(ConstantValue value)
        {
            try
            {
                return value.As<TResult>();
            }
            catch (InvalidCastException e)
            {
                throw ExceptionUtilities.RuntimeError(CurrentModule.Name, e.Message, e);
            }
        }

        private void LoadImage(ImmutableArray<ConstantValue> args)
        {
        }

        private void SetAlias(ImmutableArray<ConstantValue> args)
        {
        }

        private void Wait(ImmutableArray<ConstantValue> args)
        {
            int msDelay = Argument<int>(args[0]);
            var delay = TimeSpan.FromMilliseconds(msDelay);

            _exports.Wait(delay);
        }

        private void WaitKey(ImmutableArray<ConstantValue> args)
        {
            if (args.Length > 0)
            {
                int msTimeout = Argument<int>(args[0]);
                var timeout = TimeSpan.FromMilliseconds(msTimeout);

                _exports.WaitForClick(timeout);
            }
            else
            {
                _exports.WaitForClick();
            }
        }

        private void CreateTexture(ImmutableArray<ConstantValue> args)
        {
            string symbol = Argument<string>(args[0]);
            int zLevel = Argument<int>(args[1]);
            int x = Argument<int>(args[2]);
            int y = Argument<int>(args[3]);
            string fileName = Argument<string>(args[4]);

            _exports.DrawTexture(symbol, zLevel, x, y, fileName);
        }

        private void CreateSound(ImmutableArray<ConstantValue> args)
        {
            string symbol = Argument<string>(args[0]);
            string strAudioKind = Argument<string>(args[1]);
            AudioKind kind;
            switch (strAudioKind)
            {
                case "SE":
                    kind = AudioKind.SoundEffect;
                    break;

                case "BGM":
                default:
                    kind = AudioKind.BackgroundMusic;
                    break;
            }

            string fileName = Argument<string>(args[2]);
            _exports.LoadAudio(symbol, kind, fileName);
        }

        private void CreateColor(ImmutableArray<ConstantValue> args)
        {
            string symbol = Argument<string>(args[0]);
            int zLevel = Argument<int>(args[1]);
            int x = Argument<int>(args[2]);
            int y = Argument<int>(args[3]);
            int width = Argument<int>(args[4]);
            int height = Argument<int>(args[5]);
            string colorName = Argument<string>(args[6]);

            _exports.DrawRectangle(symbol, zLevel, x, y, width, height, colorName);
        }
    }
}

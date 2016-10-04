using System.Collections.Immutable;

namespace SciAdvNet.NSScript
{
    public abstract class Statement : SyntaxNode
    {
    }

    public static class StatementFactory
    {
        public static Chapter Chapter(Identifier name, Block body) => new Chapter(name, body);
        public static Scene Scene(Identifier name, Block body) => new Scene(name, body);

        public static Method Method(Identifier name, ImmutableArray<ParameterReference> parameters, Block body) =>
            new Method(name, parameters, body);

        public static Block Block(ImmutableArray<Statement> statements) => new Block(statements);

        public static ExpressionStatement ExpressionStatement(Expression expression) =>
            new ExpressionStatement(expression);

        public static IfStatement If(Expression condition, Statement ifTrueStatement, Statement ifFalseStatement) =>
            new IfStatement(condition, ifTrueStatement, ifFalseStatement);

        public static WhileStatement While(Expression condition, Statement body) =>
            new WhileStatement(condition, body);

        public static ReturnStatement Return() => new ReturnStatement();

        public static SelectStatement Select(Block body) => new SelectStatement(body);
        public static SelectSection SelectSection(Identifier label, Block body) => new SelectSection(label, body);

        public static CallChapterStatement CallChapter(Identifier chapterName) => new CallChapterStatement(chapterName);
        public static CallSceneStatement CallScene(Identifier sceneName) => new CallSceneStatement(sceneName);

        public static DialogueBlock DialogueBlock(string blockIdentifier, string boxName, ImmutableArray<IDialogueBlockPart> parts) =>
            new DialogueBlock(blockIdentifier, boxName, parts);

        public static DialogueLine DialogueLine(ImmutableArray<TextSegment> segments) =>
            new DialogueLine(segments);

        public static TextSegment TextSegment(string text, int colorCode) =>
            new TextSegment(text, colorCode);

        public static Voice Voice(string characterName, string fileName) =>
            new Voice(characterName, fileName);
    }

    public sealed class Chapter : Statement
    {
        internal Chapter(Identifier name, Block body)
        {
            Name = name;
            Body = body;
        }

        public Identifier Name { get; }
        public Block Body { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.Chapter;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitChapter(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitChapter(this);
        }
    }

    public sealed class Scene : Statement
    {
        internal Scene(Identifier name, Block body)
        {
            Name = name;
            Body = body;
        }

        public Identifier Name { get; }
        public Block Body { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.Scene;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitScene(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitScene(this);
        }
    }

    public sealed class Method : Statement
    {
        internal Method(Identifier name, ImmutableArray<ParameterReference> parameters, Block body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }

        public Identifier Name { get; }
        public ImmutableArray<ParameterReference> Parameters { get; }
        public Block Body { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.Method;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMethod(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitMethod(this);
        }
    }

    public sealed class Block : Statement, IDialogueBlockPart
    {
        internal Block(ImmutableArray<Statement> statements)
        {
            Statements = statements;
        }

        public ImmutableArray<Statement> Statements { get; }
        public override SyntaxNodeKind Kind => SyntaxNodeKind.Block;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBlock(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitBlock(this);
        }
    }

    public class ExpressionStatement : Statement
    {
        internal ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
        public override SyntaxNodeKind Kind => SyntaxNodeKind.ExpressionStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitExpressionStatement(this);
        }
    }

    public sealed class IfStatement : Statement
    {
        internal IfStatement(Expression condition, Statement ifTrueStatement, Statement ifFalseStatement)
        {
            Condition = condition;
            IfTrueStatement = ifTrueStatement;
            IfFalseStatement = ifFalseStatement;
        }

        public Expression Condition { get; }
        public Statement IfTrueStatement { get; }
        public Statement IfFalseStatement { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.IfStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIfStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitIfStatement(this);
        }
    }

    public sealed class WhileStatement : Statement
    {
        internal WhileStatement(Expression condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }

        public Expression Condition { get; }
        public Statement Body { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.WhileStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitWhileStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitWhileStatement(this);
        }
    }

    public sealed class ReturnStatement : Statement
    {
        public override SyntaxNodeKind Kind => SyntaxNodeKind.ReturnStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitReturnStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitReturnStatement(this);
        }
    }

    public sealed class SelectStatement : Statement
    {
        internal SelectStatement(Block body)
        {
            Body = body;
        }

        public Block Body { get; }
        public override SyntaxNodeKind Kind => SyntaxNodeKind.SelectStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSelectStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitSelectStatement(this);
        }
    }

    public sealed class SelectSection : Statement
    {
        internal SelectSection(Identifier label, Block body)
        {
            Label = label;
            Body = body;
        }

        public Identifier Label { get; }
        public Block Body { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.SelectSection;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSelectSection(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitSelectSection(this);
        }
    }

    public sealed class CallChapterStatement : Statement
    {
        internal CallChapterStatement(Identifier chapterName)
        {
            ChapterName = chapterName;
        }

        public Identifier ChapterName { get; }
        public override SyntaxNodeKind Kind => SyntaxNodeKind.CallChapterStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCallChapterStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitCallChapterStatement(this);
        }
    }

    public sealed class CallSceneStatement : Statement
    {
        internal CallSceneStatement(Identifier sceneName)
        {
            SceneName = sceneName;
        }

        public Identifier SceneName { get; }
        public override SyntaxNodeKind Kind => SyntaxNodeKind.CallSceneStatement;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCallSceneStatement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitCallSceneStatement(this);
        }
    }

    public sealed class DialogueBlock : Statement
    {
        internal DialogueBlock(string blockIdentifier, string boxName, ImmutableArray<IDialogueBlockPart> parts)
        {
            Identifier = blockIdentifier;
            BoxName = boxName;
            Parts = parts;
        }

        public string Identifier { get; }
        public string BoxName { get; }
        public ImmutableArray<IDialogueBlockPart> Parts { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.DialogueBlock;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitDialogueBlock(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitDialogueBlock(this);
        }
    }

    public interface IDialogueBlockPart
    {
    }

    public sealed class DialogueLine : Statement, IDialogueBlockPart
    {
        internal DialogueLine(ImmutableArray<TextSegment> segments)
        {
            Segments = segments;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.DialogueLine;
        public ImmutableArray<TextSegment> Segments { get; }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitDialogueLine(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitDialogueLine(this);
        }
    }

    public sealed class TextSegment : Statement, IDialogueBlockPart
    {
        internal TextSegment(string text, int colorCode)
        {
            Text = text;
            ColorCode = colorCode;
        }

        public string Text { get; }
        public int ColorCode { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.TextSegment;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitTextSegment(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitTextSegment(this);
        }
    }

    public sealed class Voice : Statement, IDialogueBlockPart
    {
        internal Voice(string characterName, string fileName)
        {
            CharacterName = characterName;
            FileName = fileName;
        }

        public string CharacterName { get; }
        public string FileName { get; }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.Voice;

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitVoice(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitVoice(this);
        }
    }
}

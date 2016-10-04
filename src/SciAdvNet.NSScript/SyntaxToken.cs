namespace SciAdvNet.NSScript
{
    public sealed class SyntaxToken
    {
        internal SyntaxToken(SyntaxTokenKind kind, string text, object value)
        {
            Kind = kind;
            Text = text;
            Value = value;
        }

        internal SyntaxToken(SyntaxTokenKind kind, string text)
            : this(kind, text, null)
        {
        }


        public SyntaxTokenKind Kind { get; }
        public string Text { get; }
        public object Value { get; }

        public bool IsXmlToken
        {
            get
            {
                switch (Kind)
                {
                    case SyntaxTokenKind.XmlElementStartTag:
                    case SyntaxTokenKind.XmlElementEndTag:
                    case SyntaxTokenKind.Xml_TextStartTag:
                    case SyntaxTokenKind.Xml_Text:
                    case SyntaxTokenKind.Xml_LineBreak:
                        return true;

                    default:
                        return false;
                }
            }
        }
    }

    public enum SyntaxTokenKind
    {
        None,
        IncludeDirective,

        IdentifierToken,
        NumericLiteralToken,
        StringLiteralToken,

        // Punctuation
        OpenBraceToken,
        CloseBraceToken,
        OpenParenToken,
        CloseParenToken,
        CommaToken,
        DotToken,
        ColonToken,
        SemicolonToken,
        EqualsToken,
        PlusToken,
        MinusToken,
        AsteriskToken,
        SlashToken,
        LessThanToken,
        GreaterThanToken,
        ExclamationToken,

        // Compound punctuation
        EqualsEqualsToken,
        PlusPlusToken,
        MinusMinusToken,
        PlusEqualsToken,
        MinusEqualsToken,
        AsteriskEqualsToken,
        SlashEqualsToken,
        LessThanEqualsToken,
        GreaterThanEqualsToken,
        ExclamationEqualsToken,
        BarBarToken,
        AmpersandAmpersandToken,

        // Keywords
        ChapterKeyword,
        FunctionKeyword,
        SceneKeyword,
        CallSceneKeyword,
        CallChapterKeyword,
        NullKeyword,
        TrueKeyword,
        FalseKeyword,
        WhileKeyword,
        IfKeyword,
        ElseKeyword,
        SelectKeyword,
        CaseKeyword,
        BreakKeyword,
        ReturnKeyword,

        // XML tokens
        XmlElementStartTag,
        XmlElementEndTag,
        Xml_TextStartTag,
        Xml_Text,
        Xml_LineBreak,

        EndOfFileToken
    }
}

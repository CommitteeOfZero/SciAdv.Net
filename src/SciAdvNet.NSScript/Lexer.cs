using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace SciAdvNet.NSScript
{
    internal enum LexerMode
    {
        Normal,
        XmlSyntax
    }

    internal sealed class Lexer
    {
        // Indicates that we've reached the end of the stream.
        // Not a valid UTF-16 character.
        private const char EofCharacter = char.MaxValue;

        private static readonly Encoding s_defaultEncoding;

        // A marker that we set at the beginning of each lexeme.
        private int _lexemeStart;
        private bool _insideDialogueBlock;
        private bool _scanningMethodSignature;

        static Lexer()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            s_defaultEncoding = Encoding.GetEncoding("shift-jis");
        }

        public Lexer(string sourceText)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            SourceText = sourceText;
        }

        public Lexer(string fileName, Stream stream)
        {
            FileName = fileName;
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream must support Read() operation.");
            }

            using (var reader = new StreamReader(stream, s_defaultEncoding, true, 4096, leaveOpen: true))
            {
                SourceText = reader.ReadToEnd();
            }
        }

        public string FileName { get; }
        public string SourceText { get; }
        public int Position { get; private set; }
        public LexerMode Mode { get; private set; } = LexerMode.Normal;

        /// <summary>
        /// Gets the current lexeme, which is the characters between the lexemeStart marker and the current position.
        /// </summary>
        private string CurrentLexeme
        {
            get
            {
                return CurrentLexemeLength != 0 ? SourceText.Substring(_lexemeStart, Position - _lexemeStart) : string.Empty;
            }
        }

        private int CurrentLexemeLength => Position - _lexemeStart;

        private char PeekChar() => PeekChar(0);
        private char PeekChar(int offset)
        {
            if (Position + offset >= SourceText.Length)
            {
                return EofCharacter;
            }

            return SourceText[Position + offset];
        }

        private void AdvanceChar() => Position++;
        private void AdvanceChar(int n) => Position += n;

        /// <summary>
        /// Sets a marker at the current position.
        /// </summary>
        private void StartScanning() => _lexemeStart = Position;

        public SyntaxToken Lex()
        {
            switch (Mode)
            {
                case LexerMode.Normal:
                    return LexSyntaxToken();

                case LexerMode.XmlSyntax:
                default:
                    return LexXmlToken();
            }
        }

        private SyntaxToken LexSyntaxToken()
        {
            string leadingTrivia = ScanSyntaxTrivia(isTrailing: false);

            SyntaxTokenKind kind = SyntaxTokenKind.None;
            string text = null;
            object value = null;
            StartScanning();

            char character = PeekChar();
            switch (character)
            {
                case '"':
                    if (!_scanningMethodSignature)
                    {
                        ScanStringLiteral();
                        kind = SyntaxTokenKind.StringLiteralToken;
                    }
                    else
                    {
                        ScanIdentifier();
                        kind = SyntaxTokenKind.IdentifierToken;
                    }
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    value = ScanNumericLiteral();
                    kind = SyntaxTokenKind.NumericLiteralToken;
                    break;

                case '$':
                    ScanIdentifier();
                    kind = SyntaxTokenKind.IdentifierToken;
                    break;

                case '#':
                    if (IsIncludeDirective())
                    {
                        ScanToEndOfLine();
                        kind = SyntaxTokenKind.IncludeDirective;
                    }
                    else if (IsHexNumericLiteral())
                    {
                        value = ScanNumericLiteral();
                        kind = SyntaxTokenKind.NumericLiteralToken;
                    }
                    else
                    {
                        ScanIdentifier();
                        kind = SyntaxTokenKind.IdentifierToken;
                    }
                    break;

                case '@':
                    character = PeekChar(1);
                    switch (character)
                    {
                        case '+':
                            AdvanceChar(2);
                            kind = SyntaxTokenKind.PlusToken;
                            break;

                        case '-':
                            character = PeekChar(2);
                            if (character == '>')
                            {
                                ScanIdentifier();
                                kind = SyntaxTokenKind.IdentifierToken;
                            }
                            else
                            {
                                AdvanceChar(2);
                                kind = SyntaxTokenKind.MinusToken;
                            }
                            break;

                        default:
                            if (SyntaxFacts.IsDecDigit(character))
                            {
                                value = ScanNumericLiteral();
                                kind = SyntaxTokenKind.NumericLiteralToken;
                            }
                            else
                            {
                                ScanIdentifier();
                                kind = SyntaxTokenKind.IdentifierToken;
                            }
                            break;
                    }
                    break;

                case '<':
                    char nextChar = PeekChar(1);
                    if (nextChar == '=')
                    {
                        AdvanceChar(2);
                        kind = SyntaxTokenKind.LessThanEqualsToken;
                    }

                    // If the next character after '<' is a latin letter, it's most likely an XML tag.
                    // I've yet to find an exception to that.
                    else if (SyntaxFacts.IsLatinLetter(nextChar))
                    {
                        kind = SyntaxTokenKind.XmlElementStartTag;
                        ScanXmlTag();

                        _insideDialogueBlock = true;
                        Mode = LexerMode.XmlSyntax;
                    }
                    else if (nextChar == '/')
                    {
                        kind = SyntaxTokenKind.XmlElementEndTag;
                        ScanXmlTag();
                    }
                    else
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.LessThanToken;
                    }
                    break;

                case '{':
                    AdvanceChar();
                    kind = SyntaxTokenKind.OpenBraceToken;
                    break;

                case '}':
                    AdvanceChar();
                    kind = SyntaxTokenKind.CloseBraceToken;
                    if (_insideDialogueBlock)
                    {
                        Mode = LexerMode.XmlSyntax;
                    }
                    break;

                case '(':
                    AdvanceChar();
                    kind = SyntaxTokenKind.OpenParenToken;
                    break;

                case ')':
                    AdvanceChar();
                    kind = SyntaxTokenKind.CloseParenToken;

                    if (_scanningMethodSignature)
                    {
                        _scanningMethodSignature = false;
                    }
                    break;

                case '.':
                    AdvanceChar();
                    kind = SyntaxTokenKind.DotToken;
                    break;

                case ',':
                    AdvanceChar();
                    kind = SyntaxTokenKind.CommaToken;
                    break;

                case ':':
                    AdvanceChar();
                    kind = SyntaxTokenKind.ColonToken;
                    break;

                case ';':
                    AdvanceChar();
                    kind = SyntaxTokenKind.SemicolonToken;
                    break;

                case '=':
                    AdvanceChar();
                    if ((character = PeekChar()) == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.EqualsEqualsToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.EqualsToken;
                    }
                    break;

                case '+':
                    AdvanceChar();
                    if ((character = PeekChar()) == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.PlusEqualsToken;
                    }
                    else if (character == '+')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.PlusPlusToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.PlusToken;
                    }
                    break;

                case '-':
                    AdvanceChar();
                    if ((character = PeekChar()) == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.MinusEqualsToken;
                    }
                    else if (character == '-')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.MinusMinusToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.MinusToken;
                    }
                    break;

                case '*':
                    AdvanceChar();
                    if (PeekChar() == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.AsteriskEqualsToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.AsteriskToken;
                    }
                    break;

                case '/':
                    AdvanceChar();
                    if (PeekChar() == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.SlashEqualsToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.SlashToken;
                    }
                    break;

                case '>':
                    AdvanceChar();
                    if (PeekChar() == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.GreaterThanEqualsToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.GreaterThanToken;
                    }
                    break;

                case '!':
                    AdvanceChar();
                    if (PeekChar() == '=')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.ExclamationEqualsToken;
                    }
                    else
                    {
                        kind = SyntaxTokenKind.ExclamationToken;
                    }
                    break;

                case '|':
                    AdvanceChar();
                    if (PeekChar() == '|')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.BarBarToken;
                    }
                    break;

                case '&':
                    AdvanceChar();
                    if (PeekChar() == '&')
                    {
                        AdvanceChar();
                        kind = SyntaxTokenKind.AmpersandAmpersandToken;
                    }
                    break;

                case EofCharacter:
                    kind = SyntaxTokenKind.EndOfFileToken;
                    break;

                default:
                    ScanIdentifier();
                    string identifier = CurrentLexeme;
                    kind = SyntaxFacts.GetKeywordKind(identifier);
                    switch (kind)
                    {
                        case SyntaxTokenKind.None:
                            kind = SyntaxTokenKind.IdentifierToken;
                            break;

                        case SyntaxTokenKind.NullKeyword:
                            value = null;
                            break;

                        case SyntaxTokenKind.TrueKeyword:
                            value = true;
                            break;

                        case SyntaxTokenKind.FalseKeyword:
                            value = false;
                            break;

                        case SyntaxTokenKind.FunctionKeyword:
                            _scanningMethodSignature = true;
                            break;
                    }
                    break;
            }

            text = CurrentLexeme;
            if (kind == SyntaxTokenKind.StringLiteralToken)
            {
                value = text.Substring(1, text.Length - 2);
            }

            string trailingTrivia = ScanSyntaxTrivia(isTrailing: true);
            return new SyntaxToken(kind, text, value);
        }

        private SyntaxToken LexXmlToken()
        {
            string leadingTrivia = ScanSyntaxTrivia(isTrailing: false);
            SyntaxTokenKind kind = SyntaxTokenKind.None;

            StartScanning();
            char character = PeekChar();
            switch (character)
            {
                case '<':
                    if (PeekChar(1) == '/')
                    {
                        // If the current lexeme is '</PRE>'
                        if (PeekChar(2) == 'P' && PeekChar(3) == 'R' && PeekChar(4) == 'E')
                        {
                            // Then we reached the end of the dialogue block.
                            _insideDialogueBlock = false;
                            Mode = LexerMode.Normal;
                        }

                        ScanXmlTag();
                        kind = SyntaxTokenKind.XmlElementEndTag;
                    }
                    else
                    {
                        ScanXmlTag();
                        kind = SyntaxTokenKind.XmlElementStartTag;
                    }
                    break;

                case '[':
                    while ((character = PeekChar()) != ']' && character != EofCharacter)
                    {
                        AdvanceChar();
                    }

                    AdvanceChar();
                    kind = SyntaxTokenKind.Xml_TextStartTag;
                    break;

                case '\r':
                case '\n':
                    while (SyntaxFacts.IsNewLine(PeekChar()))
                    {
                        ScanEndOfLine();
                    }

                    kind = SyntaxTokenKind.Xml_LineBreak;
                    break;

                case '{':
                    AdvanceChar();
                    Mode = LexerMode.Normal;
                    kind = SyntaxTokenKind.OpenBraceToken;
                    break;

                case EofCharacter:
                    kind = SyntaxTokenKind.EndOfFileToken;
                    break;

                default:
                    ScanText();
                    kind = SyntaxTokenKind.Xml_Text;
                    break;
            }

            string text = CurrentLexeme;
            string trailingTrivia = ScanSyntaxTrivia(isTrailing: true);
            return new SyntaxToken(kind, text);
        }

        private void ScanIdentifier()
        {
            char character = PeekChar();
            bool startsWithQuoteChar = false;
            switch (character)
            {
                case '@':
                    AdvanceChar();
                    // @->
                    if ((character = PeekChar()) == '-' && PeekChar(1) == '>')
                    {
                        AdvanceChar(2);
                    }
                    break;

                case '$':
                case '#':
                    AdvanceChar();
                    break;

                case '"':
                    AdvanceChar();
                    startsWithQuoteChar = true;
                    break;
            }

            char c;
            while (SyntaxFacts.IsIdentifierPartCharacter((c = PeekChar())) && c != EofCharacter)
            {
                AdvanceChar();
            }

            if (startsWithQuoteChar)
            {
                AdvanceChar();
            }
        }

        private void ScanStringLiteral()
        {
            AdvanceChar();
            char c;
            while ((c = PeekChar()) != '"' && c != EofCharacter)
            {
                AdvanceChar();
            }

            AdvanceChar();
        }

        private int ScanNumericLiteral()
        {
            char character = PeekChar();
            bool isHex = character == '#';
            bool isPrefixedByAt = character == '@';
            if (!isHex)
            {
                if (isPrefixedByAt)
                {
                    AdvanceChar();
                    if ((character = PeekChar()) == '-' || character == '+')
                    {
                        AdvanceChar();
                    }
                }

                char c;
                while (SyntaxFacts.IsDecDigit((c = PeekChar())) && c != EofCharacter)
                {
                    AdvanceChar();
                }
            }
            else
            {
                char c;
                while (SyntaxFacts.IsHexDigit((c = PeekChar())) && c != EofCharacter)
                {
                    AdvanceChar();
                }
            }

            string strValue = CurrentLexeme;
            if (isHex)
            {
                strValue = strValue.Replace("#", "0x");
            }
            else if (isPrefixedByAt)
            {
                strValue = strValue.Substring(1);
            }

            return int.Parse(strValue, isHex ? NumberStyles.AllowHexSpecifier : NumberStyles.AllowLeadingSign);
        }

        private bool IsIncludeDirective()
        {
            string include = "#include";
            for (int i = 0; i < include.Length; i++)
            {
                if (PeekChar(i) != include[i])
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsHexNumericLiteral()
        {
            // In NSS, hex literals are always 3 bytes long.
            // So we need to check if the first 6 characters are valid hex digits
            // and then make sure those are followed by a stop character (whitespace/comma/etc).

            bool result = false;
            for (int n = 1; n < 7; n++)
            {
                char c = PeekChar(n);
                if (c == EofCharacter)
                {
                    return false;
                }

                if (!SyntaxFacts.IsHexDigit(c))
                {
                    result = false;
                }
            }

            if (!SyntaxFacts.IsIdentifierStopCharacter(PeekChar(7)))
            {
                result = false;
            }

            return result;
        }

        private void ScanXmlTag()
        {
            char c;
            while ((c = PeekChar()) != '>' && c != EofCharacter)
            {
                AdvanceChar();
            }

            AdvanceChar();
        }

        private void ScanText()
        {
            while (true)
            {
                switch (PeekChar())
                {
                    case '<':
                    case '{':
                    case EofCharacter:
                        return;

                    case '/':
                        if (PeekChar(1) == '/')
                        {
                            return;
                        }
                        AdvanceChar();
                        break;

                    case '\r':
                    case '\n':
                        if (SyntaxFacts.IsNewLine(PeekChar(2)))
                        {
                            // We have a blank line (\r\n\r\n).
                            return;
                        }
                        else
                        {
                            ScanEndOfLine();
                        }
                        break;

                    default:
                        AdvanceChar();
                        break;
                }
            }
        }

        private string ScanSyntaxTrivia(bool isTrailing)
        {
            StartScanning();
            bool trivia = true;
            do
            {
                char character = PeekChar();
                if (SyntaxFacts.IsWhitespace(character))
                {
                    ScanWhitespace();
                    continue;
                }

                if (SyntaxFacts.IsNewLine(character))
                {
                    // If we're inside a dialogue block, blank lines aren't considered trivia.
                    if (_insideDialogueBlock && SyntaxFacts.IsNewLine(PeekChar(2)))
                    {
                        break;
                    }

                    ScanEndOfLine();
                    if (isTrailing)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                switch (character)
                {
                    case '/':
                        if ((character = PeekChar(1)) == '/')
                        {
                            ScanToEndOfLine();
                        }
                        else if (character == '*')
                        {
                            ScanMultiLineComment();
                        }
                        else
                        {
                            trivia = false;
                        }
                        break;

                    // Lines starting with '.' are pretty common.
                    // Treat them as single line comments, unless we're in the XmlSyntax mode.
                    case '.':
                        if (Mode == LexerMode.XmlSyntax)
                        {
                            ScanToEndOfLine();
                        }
                        else if (_insideDialogueBlock)
                        {
                            trivia = false;
                        }
                        else
                        {
                            ScanToEndOfLine();
                        }
                        break;

                    case '>':
                        character = PeekChar(1);
                        if (SyntaxFacts.IsNewLine(character) || (character == '/' && PeekChar(2) == '/'))
                        {
                            ScanToEndOfLine();
                        }
                        else
                        {
                            trivia = false;
                        }
                        break;

                    default:
                        trivia = false;
                        break;
                }
            } while (trivia);

            return CurrentLexeme;
        }

        private void ScanWhitespace()
        {
            char c;
            while (SyntaxFacts.IsWhitespace((c = PeekChar())) && c != EofCharacter)
            {
                AdvanceChar();
            }
        }
        private void ScanToEndOfLine()
        {
            char c;
            while (!SyntaxFacts.IsNewLine((c = PeekChar())) && c != EofCharacter)
            {
                AdvanceChar();
            }
        }

        private void ScanEndOfLine()
        {
            char c = PeekChar();
            switch (c)
            {
                case '\r':
                    AdvanceChar();
                    if (PeekChar() == '\n')
                    {
                        AdvanceChar();
                    }
                    break;

                case '\n':
                    AdvanceChar();
                    break;

                default:
                    if (SyntaxFacts.IsNewLine(c))
                    {
                        AdvanceChar();
                    }
                    break;
            }
        }

        private void ScanMultiLineComment()
        {
            char c;
            while (!((c = PeekChar()) == '*' && PeekChar(1) == '/') && c != EofCharacter)
            {
                AdvanceChar();
            }

            AdvanceChar(2);
        }
    }
}

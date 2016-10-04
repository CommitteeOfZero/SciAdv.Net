using SciAdvNet.Common;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Text;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// Transforms an encoded SC3 string into the intermediate representation.
    /// </summary>
    public sealed class SC3StringDecoder
    {
        private sealed class Scanner
        {
            public Scanner(BinaryReader reader)
            {
                Reader = reader;
            }

            public BinaryReader Reader { get; }
            public bool ReachedEnd => Reader.BaseStream.Position >= Reader.BaseStream.Length;

            public byte PeekByte() => Reader.PeekByte();
            public void Advance() => Reader.BaseStream.Position++;
            public void EatMarker(byte expectedMarker)
            {
                byte actualByte = Reader.PeekByte();
                if (actualByte != expectedMarker)
                {
                    throw ExceptionUtils.SC3String_UnexpectedByte(actualByte);
                }

                Advance();
            }
        }

        private readonly GameSpecificData _data;

        public SC3StringDecoder(SC3Game game)
        {
            _data = GameSpecificData.For(game);
        }

        private string Charset => _data.CharacterSet;
        private ImmutableDictionary<int, string> PrivateUseCharacters => _data.PrivateUseCharacters;
        
        private static bool IsCharacter(byte b) => b >= 0x80 || b == StringSegmentCodes.LineBreak;

        public SC3String DecodeString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                var scanner = new Scanner(reader);
                var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
                byte peek;
                while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != StringSegmentCodes.StringTerminator)
                {
                    var seg = NextSegment(scanner);
                    segments.Add(seg);
                }

                if (!scanner.ReachedEnd)
                {
                    scanner.EatMarker(StringSegmentCodes.StringTerminator);
                }

                return new SC3String(segments.ToImmutable());
            }
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            byte peek = scanner.PeekByte();
            switch (peek)
            {
                case StringSegmentCodes.CharacterName:
                    scanner.Advance();
                    return new Marker(MarkerKind.CharacterName);

                case StringSegmentCodes.DialogueLine:
                    scanner.Advance();
                    return new Marker(MarkerKind.DialogueLine);

                case StringSegmentCodes.RubyBase:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyBase);

                case StringSegmentCodes.RubyTextStart:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyTextStart);

                case StringSegmentCodes.RubyTextEnd:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyTextEnd);

                case StringSegmentCodes.SetColor:
                    scanner.Advance();
                    var colorIndex = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new SetColorCommand(colorIndex);

                case StringSegmentCodes.SetFontSize:
                    scanner.Advance();
                    int fontSize = scanner.Reader.ReadInt16();
                    return new SetFontSizeCommand(fontSize);

                case StringSegmentCodes.SetAlignment_Center:
                    scanner.Advance();
                    return new CenterTextCommand();

                case StringSegmentCodes.SetTopMargin:
                    scanner.Advance();
                    int topMargin = scanner.Reader.ReadInt16();
                    return new SetMarginCommand(null, topMargin);

                case StringSegmentCodes.SetLeftMargin:
                    scanner.Advance();
                    int leftMargin = scanner.Reader.ReadInt16();
                    return new SetMarginCommand(leftMargin, null);

                case StringSegmentCodes.EvaluateExpression:
                    scanner.Advance();
                    var expression = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new EvaluateExpressionCommand(expression);

                case StringSegmentCodes.Present:
                    scanner.Advance();
                    return new PresentCommand(resetTextAlignment: false);

                case StringSegmentCodes.Present_ResetAlignment:
                    scanner.Advance();
                    return new PresentCommand(resetTextAlignment: true);

                default:
                    peek = scanner.PeekByte();
                    if (IsCharacter(peek))
                    {
                        string value = DecodeCharacters(scanner);
                        return new TextSegment(value);
                    }
                    else
                    {
                        throw ExceptionUtils.SC3String_UnexpectedByte(peek);
                    }
            }
        }

        private string DecodeCharacters(Scanner scanner)
        {
            var sb = new StringBuilder();
            byte peek;
            while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != StringSegmentCodes.StringTerminator)
            {
                if (peek >= 0x80)
                {
                    NextCharacter(scanner, sb);
                }
                else if (peek == StringSegmentCodes.LineBreak)
                {
                    scanner.Advance();
                    sb.Append("\n");
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        private void NextCharacter(Scanner scanner, StringBuilder sb)
        {
            int index = ((scanner.Reader.ReadByte() & 0x7F) * 256) + scanner.Reader.ReadByte();
            char c = Charset[index];
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.PrivateUse)
            {
                sb.Append(c);
            }
            else
            {
                int codePoint = c;
                sb.Append($"[{PrivateUseCharacters[codePoint]}]");
            }
        }
    }
}

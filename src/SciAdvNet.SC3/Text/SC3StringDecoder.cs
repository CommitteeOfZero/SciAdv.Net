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
        
        private static bool IsCharacter(byte b) => b >= 0x80 || b == SC3StringMarker.LineBreak;

        public SC3String DecodeString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                var scanner = new Scanner(reader);
                var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
                byte peek;
                while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != SC3StringMarker.StringTerminator)
                {
                    var seg = NextSegment(scanner);
                    segments.Add(seg);
                }

                if (!scanner.ReachedEnd)
                {
                    scanner.EatMarker(SC3StringMarker.StringTerminator);
                }

                return new SC3String(segments.ToImmutable());
            }
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            byte peek = scanner.PeekByte();
            switch (peek)
            {
                case SC3StringMarker.CharacterName:
                    scanner.Advance();
                    return new Marker(MarkerKind.CharacterName);

                case SC3StringMarker.DialogueLine:
                    scanner.Advance();
                    return new Marker(MarkerKind.DialogueLine);

                case SC3StringMarker.RubyBase:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyBase);

                case SC3StringMarker.RubyTextStart:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyTextStart);

                case SC3StringMarker.RubyTextEnd:
                    scanner.Advance();
                    return new Marker(MarkerKind.RubyTextEnd);

                case SC3StringMarker.SetColor:
                    scanner.Advance();
                    var colorIndex = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new SetColorCommand(colorIndex);

                case SC3StringMarker.SetFontSize:
                    scanner.Advance();
                    int fontSize = scanner.Reader.ReadInt16();
                    return new SetFontSizeCommand(fontSize);

                case SC3StringMarker.SetAlignment_Center:
                    scanner.Advance();
                    return new CenterTextCommand();

                case SC3StringMarker.SetTopMargin:
                    scanner.Advance();
                    int topMargin = scanner.Reader.ReadInt16();
                    return new SetMarginCommand(null, topMargin);

                case SC3StringMarker.SetLeftMargin:
                    scanner.Advance();
                    int leftMargin = scanner.Reader.ReadInt16();
                    return new SetMarginCommand(leftMargin, null);

                case SC3StringMarker.EvaluateExpression:
                    scanner.Advance();
                    var expression = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new EvaluateExpressionCommand(expression);

                case SC3StringMarker.Present:
                    scanner.Advance();
                    return new PresentCommand(resetTextAlignment: false);

                case SC3StringMarker.Present_ResetAlignment:
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
            while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != SC3StringMarker.StringTerminator)
            {
                if (peek >= 0x80)
                {
                    NextCharacter(scanner, sb);
                }
                else if (peek == SC3StringMarker.LineBreak)
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

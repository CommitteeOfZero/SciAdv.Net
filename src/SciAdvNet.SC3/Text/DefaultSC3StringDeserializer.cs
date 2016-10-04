using SciAdvNet.SC3.Utils;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// The default implementation of <see cref="SC3StringDeserializer"/>.
    /// </summary>
    public class DefaultSC3StringDeserializer : SC3StringDeserializer
    {
        private sealed class Scanner
        {
            private const char EofCharacter = char.MaxValue;
            private int _textWindowStart;

            public Scanner(string text)
            {
                Text = text;
            }

            public string Text { get; }

            public bool ReachedEnd => Position >= Text.Length;
            public int Position { get; private set; }

            public string TextWindow
            {
                get
                {
                    return Text.Substring(_textWindowStart, Position - _textWindowStart);
                }
            }

            public void StartScanning() => _textWindowStart = Position;
            public char PeekChar()
            {
                if (ReachedEnd)
                {
                    return EofCharacter;
                }

                return Text[Position];
            }

            public void Advance() => Position++;
            public void EatChar(char c)
            {
                char actualCharacter = PeekChar();
                if (actualCharacter != c)
                {
                    throw new InvalidDataException();
                }

                Advance();
            }
        }

        public override SC3String Deserialize(string text)
        {
            var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
            var scanner = new Scanner(text);
            while (!scanner.ReachedEnd)
            {
                var seg = NextSegment(scanner);
                segments.Add(seg);
            }

            return new SC3String(segments.ToImmutable());
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            char peek = scanner.PeekChar();
            if (peek == '[')
            {
                var tag = ParseTag(scanner);
                return DeserializeTag(tag);
            }
            else
            {
                return ScanText(scanner);
            }
        }

        private TextSegment ScanText(Scanner scanner)
        {
            char peek;
            scanner.StartScanning();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != '[')
            {
                scanner.Advance();
            }

            string value = StringUtils.ReplaceAsciiWithFullwidth(scanner.TextWindow);
            return new TextSegment(value);
        }

        private SC3StringSegment DeserializeTag(Tag tag)
        {
            switch (tag.Name)
            {
                case "color":
                    return DeserializeColorTag(tag);
                case "margin":
                    return DeserializeMarginTag(tag);
                case "font":
                    return DeserializeFontSizeTag(tag);
                case "center":
                    return new CenterTextCommand();
                case "%p":
                    return new PresentCommand(false);
                case "%e":
                    return new PresentCommand(true);

                default:
                    return DeserializeMarker(tag);

            }
        }

        private Marker DeserializeMarker(Tag tag)
        {
            switch (tag.Name.ToLowerInvariant())
            {
                case "name":
                    return new Marker(MarkerKind.CharacterName);
                case "line":
                    return new Marker(MarkerKind.DialogueLine);
                case "rubybase":
                    return new Marker(MarkerKind.RubyBase);
                case "rubytextstart":
                    return new Marker(MarkerKind.RubyTextStart);
                case "rubytextend":
                    return new Marker(MarkerKind.RubyTextEnd);

                default:
                    throw new InvalidDataException();
            }
        }

        private SetColorCommand DeserializeColorTag(Tag tag)
        {
            string index = tag.GetAttribute("index");
            var bytes = BinaryUtils.HexStrToBytes(index);
            return new SetColorCommand(SC3ExpressionParser.ParseExpression(bytes));
        }

        private SetMarginCommand DeserializeMarginTag(Tag tag)
        {
            if (!tag.Attributes.Any())
            {
                throw new InvalidDataException();
            }

            var attr = tag.Attributes.First();
            switch (attr.Key)
            {
                case "left":
                    return new SetMarginCommand(int.Parse(attr.Value), null);

                case "top":
                    return new SetMarginCommand(null, int.Parse(attr.Value));

                default:
                    throw new InvalidDataException();
            }
        }

        private SetFontSizeCommand DeserializeFontSizeTag(Tag tag)
        {
            string strSize = tag.GetAttribute("size");
            int size = int.Parse(strSize);
            return new SetFontSizeCommand(size);
        }

        private Tag ParseTag(Scanner scanner)
        {
            scanner.EatChar('[');

            char peek;
            scanner.StartScanning();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != ']' && peek != ' ')
            {
                scanner.Advance();
            }

            string tagName = scanner.TextWindow;
            if (scanner.PeekChar() == ']')
            {
                scanner.Advance();
                return new Tag(tagName);
            }

            var attributes = ImmutableDictionary.CreateBuilder<string, string>();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != ']')
            {
                if (peek == ' ')
                {
                    scanner.Advance();
                }
                else
                {
                    var attr = ParseAttribute(scanner);
                    attributes.Add(attr);
                }
            }

            scanner.EatChar(']');
            return new Tag(tagName, attributes.ToImmutable());
        }

        private KeyValuePair<string, string> ParseAttribute(Scanner scanner)
        {
            char peek;
            scanner.StartScanning();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != '=')
            {
                scanner.Advance();
            }

            string attributeName = scanner.TextWindow;

            scanner.EatChar('=');
            scanner.EatChar('"');
            scanner.StartScanning();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != '"')
            {
                scanner.Advance();
            }

            string value = scanner.TextWindow;
            scanner.EatChar('"');
            return new KeyValuePair<string, string>(attributeName.ToLowerInvariant(), value);
        }

        private sealed class Tag
        {
            public Tag(string name, ImmutableDictionary<string, string> attributes)
            {
                Name = name;
                Attributes = attributes;
            }

            public Tag(string name)
                : this(name, ImmutableDictionary<string, string>.Empty)
            {
            }

            public string Name { get; }
            public ImmutableDictionary<string, string> Attributes { get; }

            public string GetAttribute(string attributeName)
            {
                string result;
                Attributes.TryGetValue(attributeName.ToLowerInvariant(), out result);

                if (result == null)
                {
                    throw new InvalidDataException();
                }

                return result;
            }
        }
    }
}

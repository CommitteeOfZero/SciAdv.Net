using SciAdvNet.SC3Script.Utils;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// The default implementation of <see cref="SC3StringDeserializer"/>.
    /// </summary>
    public class DefaultSC3StringDeserializer : SC3StringDeserializer
    {
        public static DefaultSC3StringDeserializer Instance { get; } = new DefaultSC3StringDeserializer();

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
            public string TextWindow => Text.Substring(_textWindowStart, Position - _textWindowStart);

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

            public (int position, int textWindowStart) RecordState() => (Position, _textWindowStart);
            public void RestoreState((int position, int textWindowStart) state)
            {
                Position = state.position;
                _textWindowStart = state.textWindowStart;
            }
        }

        public override SC3String Deserialize(string text)
        {
            var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
            var scanner = new Scanner(text);
            while (!scanner.ReachedEnd)
            {
                var seg = NextSegment(scanner);
                if (seg != null)
                {
                    segments.Add(seg);
                }
            }

            return new SC3String(segments.ToImmutable(), true);
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            char peek = scanner.PeekChar();
            if (peek == '[')
            {
                var scannerState = scanner.RecordState();

                if (TryDeserializeMarkupTag(scanner, out var segment))
                {
                    return segment;
                }

                scanner.RestoreState(scannerState);
                return ScanText(scanner);
            }
            else
            {
                return ScanText(scanner);
            }
        }

        private TextSegment ScanText(Scanner scanner)
        {
            scanner.StartScanning();
            while (!scanner.ReachedEnd)
            {
                if (scanner.PeekChar() == '[' && IsMarkupTag(scanner))
                {
                    break;
                }

                scanner.Advance();
            }

            string value = scanner.TextWindow;
            return new TextSegment(value);
        }

        private bool IsMarkupTag(Scanner scanner)
        {
            var scannerState = scanner.RecordState();
            bool result = TryDeserializeMarkupTag(scanner, out _);
            scanner.RestoreState(scannerState);
            return result;
        }

        private bool TryDeserializeMarkupTag(Scanner scanner, out SC3StringSegment outputSegment)
        {
            try
            {
                var tag = ParseMarkupTag(scanner);
                outputSegment = DeserializeTag(tag);
                return outputSegment != null;
            }
            catch (InvalidDataException)
            {
                outputSegment = null;
                return false;
            }
        }

        public virtual SC3StringSegment DeserializeTag(MarkupTag tag)
        {
            switch (tag.Name.ToLowerInvariant())
            {
                case "linebreak":
                    return new LineBreakCommand(EmbeddedCommandCodes.LineBreak);
                case "alt-linebreak":
                    return new LineBreakCommand(EmbeddedCommandCodes.AltLineBreak);
                case "name":
                    return new CharacterNameStartCommand();
                case "line":
                    return new DialogueLineStartCommand();
                case "%p":
                    return new PresentCommand(PresentCommand.SideEffectKind.None);
                case "color":
                    return DeserializeColorTag(tag);
                case "%e":
                    return new PresentCommand(PresentCommand.SideEffectKind.ResetTextAlignment);
                case "rubybase":
                case "ruby-base":
                    return new RubyBaseStartCommand();
                case "rubytextstart":
                case "ruby-text-start":
                    return new RubyTextStartCommand();
                case "rubytextend":
                case "ruby-text-end":
                    return new RubyTextEndCommand();
                case "font":
                    return DeserializeFontSizeTag(tag);
                case "parallel":
                    return new PrintInParallelCommand();
                case "center":
                    return new CenterTextCommand();
                case "margin":
                    return DeserializeMarginTag(tag);
                case "hardcodedvalue":
                case "hardcoded-value":
                    return DeserializeHardcodedValueTag(tag);
                case "autoforward":
                case "auto-forward":
                    return new AutoForwardCommand(EmbeddedCommandCodes.AutoForward);

                case "autoforward-1a":
                case "auto-forward-1a":
                    return new AutoForwardCommand(EmbeddedCommandCodes.AutoForward_1A);

                case "%18":
                    return new PresentCommand(PresentCommand.SideEffectKind.Unknown_0x18);

                case "evaluate":
                    var exprBytes = BinaryUtils.HexStringToBytes(tag.Attributes["expr"]);
                    var expr = SC3ExpressionParser.ParseExpression(exprBytes);
                    return new EvaluateExpressionCommand(expr);

                default:
                    return null;

            }
        }

        private SetColorCommand DeserializeColorTag(MarkupTag tag)
        {
            string index = tag.GetAttribute("index");
            var bytes = BinaryUtils.HexStringToBytes(index);
            return new SetColorCommand(bytes[0]);
        }

        private SetMarginCommand DeserializeMarginTag(MarkupTag tag)
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

        private SetFontSizeCommand DeserializeFontSizeTag(MarkupTag tag)
        {
            string strSize = tag.GetAttribute("size");
            int size = int.Parse(strSize);
            return new SetFontSizeCommand(size);
        }

        private GetHardcodedValueCommand DeserializeHardcodedValueTag(MarkupTag tag)
        {
            string strIndex = tag.GetAttribute("index");
            int index = int.Parse(strIndex);
            return new GetHardcodedValueCommand(index);
        }

        private MarkupTag ParseMarkupTag(Scanner scanner)
        {
            scanner.EatChar('[');

            char peek;
            scanner.StartScanning();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != ']' && !char.IsWhiteSpace(peek))
            {
                scanner.Advance();
            }

            string tagName = scanner.TextWindow;
            if (scanner.PeekChar() == ']')
            {
                scanner.Advance();
                return new MarkupTag(tagName);
            }

            var attributes = ImmutableDictionary.CreateBuilder<string, string>();
            while (!scanner.ReachedEnd && (peek = scanner.PeekChar()) != ']')
            {
                if (char.IsWhiteSpace(peek))
                {
                    scanner.Advance();
                }
                else
                {
                    var attr = ParseAttribute(scanner);
                    attributes.Add(attr);
                }
            }

            if (!scanner.ReachedEnd)
            {
                scanner.EatChar(']');
            }

            return new MarkupTag(tagName, attributes.ToImmutable());
        }

        private KeyValuePair<string, string> ParseAttribute(Scanner scanner)
        {
            scanner.StartScanning();
            while (!scanner.ReachedEnd && scanner.PeekChar() != '=')
            {
                scanner.Advance();
            }

            string attributeName = scanner.TextWindow;

            scanner.EatChar('=');
            scanner.EatChar('"');
            scanner.StartScanning();
            while (!scanner.ReachedEnd && scanner.PeekChar() != '"')
            {
                scanner.Advance();
            }

            string value = scanner.TextWindow;
            scanner.EatChar('"');
            return new KeyValuePair<string, string>(attributeName.ToLowerInvariant(), value);
        }

        public struct MarkupTag
        {
            public MarkupTag(string name, ImmutableDictionary<string, string> attributes)
            {
                Name = name;
                Attributes = attributes;
            }

            public MarkupTag(string name)
                : this(name, ImmutableDictionary<string, string>.Empty)
            {
            }

            public string Name { get; }
            public ImmutableDictionary<string, string> Attributes { get; }

            public string GetAttribute(string attributeName)
            {
                Attributes.TryGetValue(attributeName.ToLowerInvariant(), out string result);

                if (result == null)
                {
                    throw new InvalidDataException();
                }

                return result;
            }
        }
    }
}

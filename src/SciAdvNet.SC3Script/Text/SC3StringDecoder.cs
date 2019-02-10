using SciAdvNet.SC3Script.Utils;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// Produces intermediate representation of an SC3 string from a sequence of bytes.
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
            public int Position => (int)Reader.BaseStream.Position;
            public bool ReachedEnd => Reader.BaseStream.Position >= Reader.BaseStream.Length;

            public byte PeekByte() => Reader.PeekByte();
            public void Advance() => Reader.BaseStream.Position++;
            public void EatByte(byte expectedByte)
            {
                byte actualByte = Reader.PeekByte();
                if (actualByte != expectedByte)
                {
                    throw DecodingFailed(Position, unexpectedByte: actualByte);
                }

                Advance();
            }
        }

        public SC3StringDecoder(SC3Game game, string userCharacters = "")
        {
            CharacterSet = new CharacterSet(game, userCharacters);
        }

        public CharacterSet CharacterSet { get; }

        private static bool IsCharacter(byte b) => b >= 0x80 || b == EmbeddedCommandCodes.LineBreak;


        public SC3String DecodeString(ImmutableArray<byte> bytes) => DecodeString(bytes.ToArray());
        public SC3String DecodeString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                var scanner = new Scanner(reader);
                var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
                while (!scanner.ReachedEnd && (scanner.PeekByte()) != EmbeddedCommandCodes.StringTerminator)
                {
                    var seg = NextSegment(scanner);
                    segments.Add(seg);
                }

                bool isPropertyTerminated = !scanner.ReachedEnd;
                if (isPropertyTerminated)
                {
                    scanner.EatByte(EmbeddedCommandCodes.StringTerminator);
                }

                return new SC3String(segments.ToImmutable(), isPropertyTerminated);
            }
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            byte peek = scanner.PeekByte();
            switch (peek)
            {
                // case 0x1E:
                // TODO: figure out what 0x1E does

                case EmbeddedCommandCodes.LineBreak:
                case EmbeddedCommandCodes.AltLineBreak:
                    scanner.Advance();
                    return new LineBreakCommand(peek);

                case EmbeddedCommandCodes.CharacterNameStart:
                    scanner.Advance();
                    return new CharacterNameStartCommand();

                case EmbeddedCommandCodes.DialogueLineStart:
                    scanner.Advance();
                    return new DialogueLineStartCommand();

                case EmbeddedCommandCodes.Present:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.SideEffectKind.None);

                case EmbeddedCommandCodes.SetColor:
                    scanner.Advance();
                    var colorIndex = scanner.Reader.ReadByte();
                    return new SetColorCommand(colorIndex);

                case EmbeddedCommandCodes.Present_ResetAlignment:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.SideEffectKind.ResetTextAlignment);

                case EmbeddedCommandCodes.RubyBaseStart:
                    scanner.Advance();
                    return new RubyBaseStartCommand();

                case EmbeddedCommandCodes.RubyTextStart:
                    scanner.Advance();
                    return new RubyTextStartCommand();

                case EmbeddedCommandCodes.RubyTextEnd:
                    scanner.Advance();
                    return new RubyTextEndCommand();

                case EmbeddedCommandCodes.SetFontSize:
                    scanner.Advance();
                    int fontSize = scanner.Reader.ReadInt16BE();
                    return new SetFontSizeCommand(fontSize);

                case EmbeddedCommandCodes.PrintInParallel:
                    scanner.Advance();
                    return new PrintInParallelCommand();

                case EmbeddedCommandCodes.CenterText:
                    scanner.Advance();
                    return new CenterTextCommand();

                case EmbeddedCommandCodes.SetTopMargin:
                    scanner.Advance();
                    short topMargin = scanner.Reader.ReadInt16BE();
                    return new SetMarginCommand(null, topMargin);

                case EmbeddedCommandCodes.SetLeftMargin:
                    scanner.Advance();
                    short leftMargin = scanner.Reader.ReadInt16BE();
                    return new SetMarginCommand(leftMargin, null);

                case EmbeddedCommandCodes.GetHardcodedValue:
                    scanner.Advance();
                    short index = scanner.Reader.ReadInt16BE();
                    return new GetHardcodedValueCommand(index);

                case EmbeddedCommandCodes.EvaluateExpression:
                    scanner.Advance();
                    var expression = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new EvaluateExpressionCommand(expression);

                case EmbeddedCommandCodes.AutoForward:
                case EmbeddedCommandCodes.AutoForward_1A:
                    scanner.Advance();
                    return new AutoForwardCommand(peek);

                case EmbeddedCommandCodes.Present_0x18:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.SideEffectKind.Unknown_0x18);

                default:
                    peek = scanner.PeekByte();
                    if (IsCharacter(peek))
                    {
                        string value = DecodeCharacters(scanner);
                        return new TextSegment(value);
                    }
                    else
                    {
                        throw DecodingFailed(scanner.Position, unexpectedByte: peek);
                    }
            }
        }

        private string DecodeCharacters(Scanner scanner)
        {
            var sb = new StringBuilder();
            byte peek;
            while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != EmbeddedCommandCodes.StringTerminator)
            {
                if (peek >= 0x80)
                {
                    NextCharacter(scanner, sb);
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
            ushort code = scanner.Reader.ReadUInt16BE();
            char character = char.MaxValue;
            try
            {
                character = CharacterSet.DecodeCharacter(code);
                if (character != char.MaxValue)
                {
                    sb.Append(character);
                }
                else
                {
                    string compoundCharacter = CharacterSet.DecodeCompoundCharacter(code);
                    sb.Append($"[{compoundCharacter}]");
                }
            }
            catch (ArgumentException ex)
            {
                throw DecodingFailed(scanner.Position, ex.Message);
            }
        }

        private static Exception DecodingFailed(int position, string reason)
        {
            string message = $"String decoding failed. {reason}";
            return new StringDecodingFailedException(position, message);
        }

        public static Exception DecodingFailed(int position, byte unexpectedByte)
        {
            return DecodingFailed(position, $"Unexpected byte: 0x{unexpectedByte:X2}.");
        }
    }
}

using SciAdvNet.Common;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
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
                    throw DecodingFailed(unexpectedByte: actualByte);
                }

                Advance();
            }
        }

        public SC3StringDecoder(SC3Game game, string userCharacters = "")
        {
            CharacterSet = new CharacterSet(game, userCharacters);
        }

        public CharacterSet CharacterSet { get; }

        private static bool IsCharacter(byte b) => b >= 0x80 || b == EmbeddedCommands.LineBreak;


        public SC3String DecodeString(ImmutableArray<byte> bytes) => DecodeString(bytes.ToArray());
        public SC3String DecodeString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                var scanner = new Scanner(reader);
                var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
                byte peek;
                while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != EmbeddedCommands.StringTerminator)
                {
                    var seg = NextSegment(scanner);
                    segments.Add(seg);
                }

                bool isPropertyTerminated = !scanner.ReachedEnd;
                if (isPropertyTerminated)
                {
                    scanner.EatMarker(EmbeddedCommands.StringTerminator);
                }

                return new SC3String(segments.ToImmutable(), isPropertyTerminated);
            }
        }

        private SC3StringSegment NextSegment(Scanner scanner)
        {
            byte peek = scanner.PeekByte();
            switch (peek)
            {
                case 0x1E:
                    scanner.Advance();
                    scanner.Advance();
                    scanner.Advance();
                    return new LineBreakCommand();

                case 0x1F:
                    scanner.Advance();
                    return new LineBreakCommand();

                case EmbeddedCommands.LineBreak:
                    scanner.Advance();
                    return new LineBreakCommand();

                case EmbeddedCommands.CharacterNameStart:
                    scanner.Advance();
                    return new CharacterNameStartCommand();

                case EmbeddedCommands.DialogueLineStart:
                    scanner.Advance();
                    return new DialogueLineStartCommand();

                case EmbeddedCommands.Present:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.Action.None);

                case EmbeddedCommands.SetColor:
                    scanner.Advance();
                    var colorIndex = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new SetColorCommand(colorIndex);

                case EmbeddedCommands.Present_ResetAlignment:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.Action.ResetTextAlignment);

                case EmbeddedCommands.RubyBaseStart:
                    scanner.Advance();
                    return new RubyBaseStartCommand();

                case EmbeddedCommands.RubyTextStart:
                    scanner.Advance();
                    return new RubyTextStartCommand();

                case EmbeddedCommands.RubyTextEnd:
                    scanner.Advance();
                    return new RubyTextEndCommand();

                case EmbeddedCommands.SetFontSize:
                    scanner.Advance();
                    int fontSize = scanner.Reader.ReadInt16BE();
                    return new SetFontSizeCommand(fontSize);

                case EmbeddedCommands.PrintInParallel:
                    scanner.Advance();
                    return new PrintInParallelCommand();

                case EmbeddedCommands.CenterText:
                    scanner.Advance();
                    return new CenterTextCommand();

                case EmbeddedCommands.SetTopMargin:
                    scanner.Advance();
                    short topMargin = scanner.Reader.ReadInt16BE();
                    return new SetMarginCommand(null, topMargin);

                case EmbeddedCommands.SetLeftMargin:
                    scanner.Advance();
                    short leftMargin = scanner.Reader.ReadInt16BE();
                    return new SetMarginCommand(leftMargin, null);

                case EmbeddedCommands.GetHardcodedValue:
                    scanner.Advance();
                    short index = scanner.Reader.ReadInt16BE();
                    return new GetHardcodedValueCommand(index);

                case EmbeddedCommands.EvaluateExpression:
                    scanner.Advance();
                    var expression = SC3ExpressionParser.ParseExpression(scanner.Reader);
                    return new EvaluateExpressionCommand(expression);

                case EmbeddedCommands.AutoForward:
                    scanner.Advance();
                    return new AutoForwardCommand();

                case EmbeddedCommands.Present_0x18:
                    scanner.Advance();
                    return new PresentCommand(PresentCommand.Action.Unknown_0x18);

                default:
                    peek = scanner.PeekByte();
                    if (IsCharacter(peek))
                    {
                        string value = DecodeCharacters(scanner);
                        return new TextSegment(value);
                    }
                    else
                    {
                        throw DecodingFailed(unexpectedByte: peek);
                    }
            }
        }

        private string DecodeCharacters(Scanner scanner)
        {
            var sb = new StringBuilder();
            byte peek;
            while (!scanner.ReachedEnd && (peek = scanner.PeekByte()) != EmbeddedCommands.StringTerminator)
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
                throw DecodingFailed(ex.Message);
            }
        }

        private static Exception DecodingFailed(string reason)
        {
            string message = $"String decoding failed. {reason}";
            return new StringDecodingFailedException(message);
        }

        public static Exception DecodingFailed(byte unexpectedByte)
        {
            return DecodingFailed($"Unexpected byte: 0x{unexpectedByte:X2}");
        }
    }
}

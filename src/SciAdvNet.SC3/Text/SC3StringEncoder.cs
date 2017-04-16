using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// Takes the intermediate representation of an SC3 string and turns it into a sequence of bytes.
    /// </summary>
    public sealed class SC3StringEncoder : SC3StringSegmentVisitor
    {
        public SC3StringEncoder(SC3Game game, string userCharacters = "")
        {
            CharacterSet = new CharacterSet(game, userCharacters);
        }

        public CharacterSet CharacterSet { get; }

        public ImmutableArray<byte> Encode(SC3String sc3String)
        {
            var impl = new EncoderImpl(CharacterSet);
            return impl.Encode(sc3String);
        }

        private sealed class EncoderImpl : SC3StringSegmentVisitor
        {
            private readonly CharacterSet _characterSet;
            private ImmutableArray<byte>.Builder _builder;

            public EncoderImpl(CharacterSet characterSet)
            {
                _characterSet = characterSet;
            }

            public ImmutableArray<byte> Encode(SC3String sc3String)
            {
                _builder = ImmutableArray.CreateBuilder<byte>();
                foreach (var segment in sc3String.Segments)
                {
                    Visit(segment);
                }

                if (sc3String.IsProperlyTerminated)
                {
                    Write(EmbeddedCommands.StringTerminator);
                }

                return _builder.ToImmutable();
            }

            public override void VisitTextSegment(TextSegment text)
            {
                foreach (char c in text.Value)
                {
                    ushort code;
                    try
                    {
                        code = _characterSet.EncodeCharacter(c);
                        Write(code);
                    }
                    catch (ArgumentException ex)
                    {
                        throw EncodingFailed(ex.Message);
                    }
                }
            }

            public override void VisitLineBreakCommand(LineBreakCommand lineBreakCommand)
            {
                Write(EmbeddedCommands.LineBreak);
            }

            public override void VisitCharacterNameStartCommand(CharacterNameStartCommand characterNameStartCommand)
            {
                Write(EmbeddedCommands.CharacterNameStart);
            }

            public override void VisitDialogueLineStartCommand(DialogueLineStartCommand dialogueLineStartCommand)
            {
                Write(EmbeddedCommands.DialogueLineStart);
            }

            public override void VisitPresentCommand(PresentCommand presentCommand)
            {
                byte code;
                switch (presentCommand.AttachedAction)
                {
                    case PresentCommand.Action.ResetTextAlignment:
                        code = EmbeddedCommands.Present_ResetAlignment;
                        break;

                    case PresentCommand.Action.Unknown_0x18:
                        code = EmbeddedCommands.Present_0x18;
                        break;

                    case PresentCommand.Action.None:
                    default:
                        code = EmbeddedCommands.Present;
                        break;
                }

                Write(code);
            }

            public override void VisitSetColorCommand(SetColorCommand setColorCommand)
            {
                Write(EmbeddedCommands.SetColor);
                Write(setColorCommand.ColorIndex.Bytes);
            }

            public override void VisitRubyBaseStartCommand(RubyBaseStartCommand rubyBaseStartCommand)
            {
                Write(EmbeddedCommands.RubyBaseStart);
            }

            public override void VisitRubyTextStartCommand(RubyTextStartCommand rubyTextStartCommand)
            {
                Write(EmbeddedCommands.RubyTextStart);
            }

            public override void VisitRubyTextEndCommand(RubyTextEndCommand rubyTextEndCommand)
            {
                Write(EmbeddedCommands.RubyTextEnd);
            }

            public override void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand)
            {
                Write(EmbeddedCommands.SetFontSize);
                WriteInt16BE((short)setFontSizeCommand.FontSize);
            }

            public override void VisitPrintInParallelCommand(PrintInParallelCommand printInParallelCommand)
            {
                Write(EmbeddedCommands.PrintInParallel);
            }

            public override void VisitCenterTextCommand(CenterTextCommand centerTextCommand)
            {
                Write(EmbeddedCommands.CenterText);
            }

            public override void VisitSetMarginCommand(SetMarginCommand setMarginCommand)
            {
                if (setMarginCommand.LeftMargin.HasValue)
                {
                    Write(EmbeddedCommands.SetLeftMargin);
                    WriteInt16BE((short)setMarginCommand.LeftMargin.Value);
                }
                else
                {
                    Write(EmbeddedCommands.SetTopMargin);
                    WriteInt16BE((short)setMarginCommand.TopMargin.Value);
                }
            }

            public override void VisitGetHardcodedValueCommand(GetHardcodedValueCommand getHardcodedValueCommand)
            {
                Write(EmbeddedCommands.GetHardcodedValue);
                WriteInt16BE((short)getHardcodedValueCommand.Index);
            }

            public override void VisitEvaluateExpressionCommand(EvaluateExpressionCommand evaluateExpressionCommand)
            {
                base.VisitEvaluateExpressionCommand(evaluateExpressionCommand);
            }

            public override void VisitAutoForwardCommand(AutoForwardCommand autoForwardCommand)
            {
                Write(EmbeddedCommands.AutoForward);
            }

            private void Write(byte b) => _builder.Add(b);
            private void WriteInt16BE(short value)
            {
                var bytes = BitConverter.GetBytes(value);
                _builder.Add(bytes[1]);
                _builder.Add(bytes[0]);
            }

            private void Write(ushort value)
            {
                var bytes = BitConverter.GetBytes(value);
                _builder.Add(bytes[1]);
                _builder.Add(bytes[0]);
            }

            private void Write(IEnumerable<byte> bytes) => _builder.AddRange(bytes);

            private static Exception EncodingFailed(string reason)
            {
                string message = $"String encoding failed. {reason}";
                return new StringEncodingFailedException(message);
            }
        }
    }
}

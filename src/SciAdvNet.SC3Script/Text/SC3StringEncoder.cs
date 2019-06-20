using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// Takes intermediate representation of an SC3 string and turns it into a sequence of bytes.
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
                    Write(EmbeddedCommandCodes.StringTerminator);
                }

                return _builder.ToImmutable();
            }

            public override void VisitTextSegment(TextSegment text)
            {
                string value = text.Value;
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    if (c == '[')
                    {
                        int idxClosingBracket = value.IndexOf(']', i);
                        if (idxClosingBracket != -1)
                        {
                            string textInBrackets = value.Substring(i + 1, idxClosingBracket - i - 1);
                            if (_characterSet.TryEncodeCompoundCharacter(textInBrackets, out ushort code))
                            {
                                Write(code);
                                i += (idxClosingBracket - i);
                                continue;
                            }
                        }
                    }

                    try
                    {
                        ushort code = _characterSet.EncodeCharacter(c);
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
                Write(lineBreakCommand.Code);
            }

            public override void VisitCharacterNameStartCommand(CharacterNameStartCommand characterNameStartCommand)
            {
                Write(EmbeddedCommandCodes.CharacterNameStart);
            }

            public override void VisitDialogueLineStartCommand(DialogueLineStartCommand dialogueLineStartCommand)
            {
                Write(EmbeddedCommandCodes.DialogueLineStart);
            }

            public override void VisitPresentCommand(PresentCommand presentCommand)
            {
                byte code;
                switch (presentCommand.SideEffect)
                {
                    case PresentCommand.SideEffectKind.ResetTextAlignment:
                        code = EmbeddedCommandCodes.Present_ResetAlignment;
                        break;

                    case PresentCommand.SideEffectKind.Unknown_0x18:
                        code = EmbeddedCommandCodes.Present_0x18;
                        break;

                    case PresentCommand.SideEffectKind.None:
                    default:
                        code = EmbeddedCommandCodes.Present;
                        break;
                }

                Write(code);
            }

            public override void VisitSetColorCommand(SetColorCommand setColorCommand)
            {
                Write(EmbeddedCommandCodes.SetColor);
                Write(setColorCommand.ColorIndex.Bytes);
            }

            public override void VisitRubyBaseStartCommand(RubyBaseStartCommand rubyBaseStartCommand)
            {
                Write(EmbeddedCommandCodes.RubyBaseStart);
            }

            public override void VisitRubyTextStartCommand(RubyTextStartCommand rubyTextStartCommand)
            {
                Write(EmbeddedCommandCodes.RubyTextStart);
            }

            public override void VisitRubyTextEndCommand(RubyTextEndCommand rubyTextEndCommand)
            {
                Write(EmbeddedCommandCodes.RubyTextEnd);
            }

            public override void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand)
            {
                Write(EmbeddedCommandCodes.SetFontSize);
                WriteInt16BE((short)setFontSizeCommand.FontSize);
            }

            public override void VisitPrintInParallelCommand(PrintInParallelCommand printInParallelCommand)
            {
                Write(EmbeddedCommandCodes.PrintInParallel);
            }

            public override void VisitCenterTextCommand(CenterTextCommand centerTextCommand)
            {
                Write(EmbeddedCommandCodes.CenterText);
            }

            public override void VisitSetMarginCommand(SetMarginCommand setMarginCommand)
            {
                if (setMarginCommand.LeftMargin.HasValue)
                {
                    Write(EmbeddedCommandCodes.SetLeftMargin);
                    WriteInt16BE((short)setMarginCommand.LeftMargin.Value);
                }
                else
                {
                    Write(EmbeddedCommandCodes.SetTopMargin);
                    WriteInt16BE((short)setMarginCommand.TopMargin.Value);
                }
            }

            public override void VisitGetHardcodedValueCommand(GetHardcodedValueCommand getHardcodedValueCommand)
            {
                Write(EmbeddedCommandCodes.GetHardcodedValue);
                WriteInt16BE((short)getHardcodedValueCommand.Index);
            }

            public override void VisitEvaluateExpressionCommand(EvaluateExpressionCommand evaluateExpressionCommand)
            {
                Write(EmbeddedCommandCodes.EvaluateExpression);
                Write(evaluateExpressionCommand.Expression.Bytes);
            }

            public override void VisitAutoForwardCommand(AutoForwardCommand autoForwardCommand)
            {
                Write(autoForwardCommand.Code);
            }

            public override void VisitRubyCenterPerCharCommand(RubyCenterPerCharCommand rubyCenterPerCharCommand)
            {
                Write(EmbeddedCommandCodes.RubyCenterPerChar);
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

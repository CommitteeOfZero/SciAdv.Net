using SciAdvNet.SC3Script.Utils;
using System.IO;
using System;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// The default implementation of <see cref="SC3StringSerializer"/>.
    /// </summary>
    public class DefaultSC3StringSerializer : SC3StringSerializer
    {
        public static DefaultSC3StringSerializer Instance { get; } = new DefaultSC3StringSerializer();

        public override string Serialize(SC3String sc3String)
        {
            var impl = new SerializerImpl();
            return impl.Serialize(sc3String);
        }

        public override string SerializeSegment(SC3StringSegment segment)
        {
            var impl = new SerializerImpl();
            return impl.SerializeSegment(segment);
        }

        private sealed class SerializerImpl : SC3StringSegmentVisitor
        {
            private TextWriter _writer;

            private void Write(string text) => _writer.Write(text);

            public string Serialize(SC3String s)
            {
                _writer = new StringWriter();
                foreach (var segment in s.Segments)
                {
                    Visit(segment);
                }

                return _writer.ToString();
            }

            public string SerializeSegment(SC3StringSegment segment)
            {
                _writer = new StringWriter();
                Visit(segment);
                return _writer.ToString();
            }

            public override void VisitTextSegment(TextSegment text)
            {
                Write(text.Value);
            }

            public override void VisitLineBreakCommand(LineBreakCommand lineBreakCommand)
            {
                if (lineBreakCommand.Code == EmbeddedCommandCodes.LineBreak)
                {
                    Write("[linebreak]");
                }
                else if (lineBreakCommand.Code == EmbeddedCommandCodes.AltLineBreak)
                {
                    Write("[alt-linebreak]");
                }
            }

            public override void VisitCharacterNameStartCommand(CharacterNameStartCommand characterNameStartCommand)
            {
                Write("[name]");
            }

            public override void VisitDialogueLineStartCommand(DialogueLineStartCommand dialogueLineStartCommand)
            {
                Write("[line]");
            }

            public override void VisitPresentCommand(PresentCommand presentCommand)
            {
                string text;
                switch (presentCommand.SideEffect)
                {
                    case PresentCommand.SideEffectKind.ResetTextAlignment:
                        text = "[%e]";
                        break;

                    case PresentCommand.SideEffectKind.Unknown_0x18:
                        text = "[%18]";
                        break;

                    case PresentCommand.SideEffectKind.None:
                    default:
                        text = "[%p]";
                        break;
                }

                Write(text);
            }

            public override void VisitSetColorCommand(SetColorCommand setColorCommand)
            {
                string index = BinaryUtils.BytesToHexString(new[] { setColorCommand.ColorIndex });
                Write($"[color index=\"{index}\"]");
            }

            public override void VisitRubyBaseStartCommand(RubyBaseStartCommand rubyBaseStartCommand)
            {
                Write("[ruby-base]");
            }

            public override void VisitRubyTextStartCommand(RubyTextStartCommand rubyTextStartCommand)
            {
                Write("[ruby-text-start]");
            }

            public override void VisitRubyTextEndCommand(RubyTextEndCommand rubyTextEndCommand)
            {
                Write("[ruby-text-end]");
            }

            public override void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand)
            {
                Write($"[font size=\"{setFontSizeCommand.FontSize}\"]");
            }

            public override void VisitPrintInParallelCommand(PrintInParallelCommand printInParallelCommand)
            {
                Write("[parallel]");
            }

            public override void VisitCenterTextCommand(CenterTextCommand centerTextCommand)
            {
                Write("[center]");
            }

            public override void VisitSetMarginCommand(SetMarginCommand setMarginCommand)
            {
                if (setMarginCommand.LeftMargin.HasValue)
                {
                    int left = setMarginCommand.LeftMargin.Value;
                    Write($"[margin left=\"{left}\"]");
                }
                else if (setMarginCommand.TopMargin.HasValue)
                {
                    int top = setMarginCommand.TopMargin.Value;
                    Write($"[margin top=\"{top}\"]");
                }
            }

            public override void VisitGetHardcodedValueCommand(GetHardcodedValueCommand getHardcodedValueCommand)
            {
                Write($"[hardcoded-value index=\"{getHardcodedValueCommand.Index}\"]");
            }

            public override void VisitEvaluateExpressionCommand(EvaluateExpressionCommand evaluateExpressionCommand)
            {
                string expr = BinaryUtils.BytesToHexString(evaluateExpressionCommand.Expression.Bytes);
                Write($"[evaluate expr=\"{expr}\"]");
            }

            public override void VisitAutoForwardCommand(AutoForwardCommand autoForwardCommand)
            {
                if (autoForwardCommand.Code == EmbeddedCommandCodes.AutoForward)
                {
                    Write("[auto-forward]");
                }
                else if (autoForwardCommand.Code == EmbeddedCommandCodes.AutoForward_1A)
                {
                    Write("[auto-forward-1a]");
                }
            }
        }
    }
}

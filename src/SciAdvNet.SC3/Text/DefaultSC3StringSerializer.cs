using SciAdvNet.SC3.Utils;
using System.Collections.Generic;
using System.IO;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// The default implementation of <see cref="SC3StringSerializer"/>.
    /// </summary>
    public class DefaultSC3StringSerializer : SC3StringSerializer
    {
        public override string Serialize(SC3String sc3String)
        {
            var impl = new SerializerImpl();
            return impl.Serialize(sc3String);
        }

        private sealed class SerializerImpl : SC3StringSegmentVisitor
        {
            private static readonly Dictionary<MarkerKind, string> s_markers = new Dictionary<MarkerKind, string>()
            {
                [MarkerKind.CharacterName] = "[name]",
                [MarkerKind.DialogueLine] = "[line]",
                [MarkerKind.RubyBase] = "[rubyBase]",
                [MarkerKind.RubyTextStart] = "[rubyTextStart]",
                [MarkerKind.RubyTextEnd] = "[rubyTextEnd]"
            };

            private TextWriter _writer;

            public string Serialize(SC3String s)
            {
                _writer = new StringWriter();
                foreach (var segment in s.Segments)
                {
                    Visit(segment);
                }

                return _writer.ToString();
            }

            public override void VisitTextSegment(TextSegment text)
            {
                _writer.Write(text.Value);
            }

            public override void VisitMarker(Marker marker)
            {
                _writer.Write(s_markers[marker.MarkerKind]);
            }

            public override void VisitSetColorCommand(SetColorCommand setColorCommand)
            {
                string index = BinaryUtils.BytesToHexString(setColorCommand.ColorIndex.Bytes);
                _writer.Write($"[color index=\"{index}\"]");
            }

            public override void VisitPresentCommand(PresentCommand presentCommand)
            {
                string text = presentCommand.ResetTextAlignment ? "[%e]" : "[%p]";
                _writer.Write(text);
            }

            public override void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand)
            {
                _writer.Write($"[font size=\"{setFontSizeCommand.FontSize}\"]");
            }

            public override void VisitCenterTextCommand(CenterTextCommand centerTextCommand)
            {
                _writer.Write("[center]");
            }

            public override void VisitSetMarginCommand(SetMarginCommand setMarginCommand)
            {
                if (setMarginCommand.LeftMargin.HasValue)
                {
                    int left = setMarginCommand.LeftMargin.Value;
                    _writer.Write($"[margin left=\"{left}\"]");
                }
                else if (setMarginCommand.TopMargin.HasValue)
                {
                    int top = setMarginCommand.TopMargin.Value;
                    _writer.Write($"[margin top=\"{top}\"]");
                }
            }
        }
    }
}

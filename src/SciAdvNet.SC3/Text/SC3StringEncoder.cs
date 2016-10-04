using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// Takes the intermediate representation of an SC3 string and turns it into a sequence of bytes.
    /// </summary>
    public sealed class SC3StringEncoder : SC3StringSegmentVisitor
    {
        private readonly GameSpecificData _data;

        public SC3StringEncoder(SC3Game game)
        {
            _data = GameSpecificData.For(game);
        }

        public ImmutableArray<byte> Encode(SC3String sc3String)
        {
            var impl = new EncoderImpl(_data);
            return impl.Encode(sc3String);
        }

        private sealed class EncoderImpl : SC3StringSegmentVisitor
        {
            private static readonly Dictionary<MarkerKind, byte> s_markers = new Dictionary<MarkerKind, byte>()
            {
                [MarkerKind.CharacterName] = SC3StringMarker.CharacterName,
                [MarkerKind.DialogueLine] = SC3StringMarker.DialogueLine,
                [MarkerKind.RubyBase] = SC3StringMarker.RubyBase,
                [MarkerKind.RubyTextStart] = SC3StringMarker.RubyTextStart,
                [MarkerKind.RubyTextEnd] = SC3StringMarker.RubyTextEnd
            };

            private ImmutableArray<byte>.Builder _builder;
            private readonly GameSpecificData _data;
            private string Charset => _data.CharacterSet;
            private ImmutableDictionary<int, string> PrivateUseCharacters => _data.PrivateUseCharacters;

            public EncoderImpl(GameSpecificData data)
            {
                _data = data;
            }

            public ImmutableArray<byte> Encode(SC3String sc3String)
            {
                _builder = ImmutableArray.CreateBuilder<byte>();
                foreach (var segment in sc3String.Segments)
                {
                    Visit(segment);
                }

                Append(SC3StringMarker.StringTerminator);
                return _builder.ToImmutable();
            }

            public override void VisitTextSegment(TextSegment text)
            {
                foreach (char c in text.Value)
                {
                    int index = Charset.IndexOf(c.ToString(), StringComparison.Ordinal);
                    if (index == -1)
                    {
                        Debugger.Break();
                    }

                    byte first = (byte)(0x80 + (byte)(index / 256));
                    byte second = (byte)(index % 256);

                    Append(first);
                    Append(second);
                }
            }

            public override void VisitMarker(Marker marker)
            {
                Append(s_markers[marker.MarkerKind]);
            }

            public override void VisitPresentCommand(PresentCommand presentCommand)
            {
                byte b = presentCommand.ResetTextAlignment ? SC3StringMarker.Present_ResetAlignment : SC3StringMarker.Present;
                Append(b);
            }

            public override void VisitSetColorCommand(SetColorCommand setColorCommand)
            {
                Append(SC3StringMarker.SetColor);
                Append(setColorCommand.ColorIndex.Bytes);
            }

            public override void VisitCenterTextCommand(CenterTextCommand centerTextCommand)
            {
                Append(SC3StringMarker.SetAlignment_Center);
            }

            public override void VisitSetMarginCommand(SetMarginCommand setMarginCommand)
            {
                if (setMarginCommand.LeftMargin.HasValue)
                {
                    Append(SC3StringMarker.SetLeftMargin);
                    Append(BitConverter.GetBytes(setMarginCommand.LeftMargin.Value));
                }
                else
                {
                    Append(SC3StringMarker.SetTopMargin);
                    Append(BitConverter.GetBytes(setMarginCommand.TopMargin.Value));
                }
            }

            public override void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand)
            {
                Append(SC3StringMarker.SetFontSize);
                Append(BitConverter.GetBytes(setFontSizeCommand.FontSize));
            }

            private void Append(byte b) => _builder.Add(b);
            private void Append(IEnumerable<byte> bytes) => _builder.AddRange(bytes);
        }
    }
}

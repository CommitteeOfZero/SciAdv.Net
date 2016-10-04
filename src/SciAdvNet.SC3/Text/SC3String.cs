using SciAdvNet.SC3.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SciAdvNet.SC3.Text
{
    /// <summary>
    /// The intermediate representation of an SC3 string.
    /// </summary>
    public sealed class SC3String
    {
        public static readonly SC3String Empty = new SC3String(ImmutableArray<SC3StringSegment>.Empty);

        private static readonly Dictionary<SC3Game, Lazy<SC3StringEncoder>> s_encoders;
        private static readonly Dictionary<SC3Game, Lazy<SC3StringDecoder>> s_decoders;
        private static readonly DefaultSC3StringSerializer s_serializer;
        private static readonly DefaultSC3StringDeserializer s_deserializer;

        static SC3String()
        {
            s_serializer = new DefaultSC3StringSerializer();
            s_deserializer = new DefaultSC3StringDeserializer();
            s_encoders = new Dictionary<SC3Game, Lazy<SC3StringEncoder>>();
            s_decoders = new Dictionary<SC3Game, Lazy<SC3StringDecoder>>();
            foreach (var game in GameSpecificData.SupportedGames)
            {
                s_encoders[game] = new Lazy<SC3StringEncoder>(() => new SC3StringEncoder(game));
                s_decoders[game] = new Lazy<SC3StringDecoder>(() => new SC3StringDecoder(game));
            }
        }

        internal SC3String(ImmutableArray<SC3StringSegment> segments)
        {
            Segments = segments;
        }

        /// <summary>
        /// The collection of segments that this instance of <see cref="SC3String"/> consists of. 
        /// </summary>
        public ImmutableArray<SC3StringSegment> Segments { get; }

        public static SC3String FromBytes(byte[] bytes, SC3Game game)
        {
            var decoder = s_decoders[game].Value;
            return decoder.DecodeString(bytes);
        }

        public static SC3String FromHexString(string hexString, SC3Game game)
        {
            var bytes = BinaryUtils.HexStrToBytes(hexString);
            return FromBytes(bytes, game);
        }

        public static SC3String Deserialize(string text)
        {
            return s_deserializer.Deserialize(text);
        }

        public static SC3String Deserialize(string text, SC3StringDeserializer deserializer)
        {
            return deserializer.Deserialize(text);
        }

        public ImmutableArray<byte> Encode(SC3Game game)
        {
            return s_encoders[game].Value.Encode(this);
        }

        /// <summary>
        /// Extracts the dialogue line from this <see cref="SC3String"/>.
        /// Returns the whole string if the character name is not specified.
        /// </summary>
        public SC3String GetDialogueLine()
        {
            if (Segments.Length == 0)
            {
                return SC3String.Empty;
            }

            var marker = Segments[0] as Marker;
            if (marker == null || marker.MarkerKind != MarkerKind.CharacterName)
            {
                return this;
            }

            int idxNameSegment = -1;
            for (int i = 0; i < Segments.Length; i++)
            {
                if ((Segments[i] as Marker)?.MarkerKind == MarkerKind.DialogueLine)
                {
                    idxNameSegment = i;
                    break;
                }
            }

            if (idxNameSegment == -1)
            {
                return SC3String.Empty;
            }

            var segments = Segments.Skip(idxNameSegment + 1);
            return new SC3String(segments.ToImmutableArray());
        }

        /// <summary>
        /// Extracts the character name from this <see cref="SC3String"/>.
        /// Returns an emptry <see cref="SC3String"/> if the character name is not specified.
        /// </summary>
        public SC3String GetCharacterName()
        {
            if (Segments.Length == 0)
            {
                return SC3String.Empty;
            }

            var marker = Segments[0] as Marker;
            if (marker == null || marker.MarkerKind != MarkerKind.CharacterName)
            {
                return SC3String.Empty;
            }

            var segments = Segments.Skip(1).TakeWhile(s => (s as Marker)?.MarkerKind != MarkerKind.DialogueLine);
            return new SC3String(segments.ToImmutableArray());
        }

        public override string ToString()
        {
            return ToString(normalize: false);
        }

        public string ToString(bool normalize)
        {
            string s = s_serializer.Serialize(this);
            return normalize ? StringUtils.ReplaceFullwidthLatinWithAscii(s) : s;
        }
    }

    public abstract class SC3StringSegment
    {
        public abstract SC3StringSegmentKind SegmentKind { get; }

        internal abstract void Accept(SC3StringSegmentVisitor visitor);
    }

    public enum SC3StringSegmentKind
    {
        Text,
        Marker,
        EmbeddedCommand
    }

    public sealed class TextSegment : SC3StringSegment
    {
        internal TextSegment(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override SC3StringSegmentKind SegmentKind => SC3StringSegmentKind.Text;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitTextSegment(this);
        }
    }

    public sealed class Marker : SC3StringSegment
    {
        internal Marker(MarkerKind markerKind)
        {
            MarkerKind = markerKind;
        }

        public MarkerKind MarkerKind { get; }
        public override SC3StringSegmentKind SegmentKind => SC3StringSegmentKind.Marker;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitMarker(this);
        }
    }

    public enum MarkerKind
    {
        CharacterName,
        DialogueLine,
        RubyBase,
        RubyTextStart,
        RubyTextEnd
    }

    public abstract class EmbeddedCommand : SC3StringSegment
    {
        public override SC3StringSegmentKind SegmentKind => SC3StringSegmentKind.EmbeddedCommand;
        public abstract EmbeddedCommandKind CommandKind { get; }
    }

    public enum EmbeddedCommandKind
    {
        SetColor,
        SetMargin,
        CenterText,
        EvaluateExpression,
        Present,
        SetFontSize
    }

    public sealed class SetColorCommand : EmbeddedCommand
    {
        internal SetColorCommand(Expression colorIndex)
        {
            ColorIndex = colorIndex;
        }

        public Expression ColorIndex { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.SetColor;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitSetColorCommand(this);
        }
    }

    public sealed class SetFontSizeCommand : EmbeddedCommand
    {
        internal SetFontSizeCommand(int fontSize)
        {
            FontSize = fontSize;
        }
        
        public int FontSize { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.SetFontSize;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitSetFontSizeCommand(this);
        }
    }

    public sealed class CenterTextCommand : EmbeddedCommand
    {
        internal CenterTextCommand()
        {
        }

        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.CenterText;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitCenterTextCommand(this);
        }
    }

    public sealed class SetMarginCommand : EmbeddedCommand
    {
        internal SetMarginCommand(int? leftMargin, int? topMargin)
        {
            LeftMargin = leftMargin;
            TopMargin = topMargin;
        }

        public int? LeftMargin { get; }
        public int? TopMargin { get; }

        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.SetMargin;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitSetMarginCommand(this);
        }
    }

    public sealed class EvaluateExpressionCommand : EmbeddedCommand
    {
        internal EvaluateExpressionCommand(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.EvaluateExpression;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitEvaluateExpressionCommand(this);
        }
    }

    public sealed class PresentCommand : EmbeddedCommand
    {
        internal PresentCommand(bool resetTextAlignment)
        {
            ResetTextAlignment = resetTextAlignment;
        }

        public bool ResetTextAlignment { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.Present;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitPresentCommand(this);
        }
    }
}

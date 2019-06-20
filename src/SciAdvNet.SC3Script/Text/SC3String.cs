using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// Intermediate representation of an SC3 string.
    /// </summary>
    public sealed class SC3String : IEquatable<SC3String>
    {
        public static readonly SC3String Empty = new SC3String(ImmutableArray<SC3StringSegment>.Empty, false);

        private static readonly Dictionary<SC3Game, Lazy<SC3StringEncoder>> s_encoders;
        private static readonly Dictionary<SC3Game, Lazy<SC3StringDecoder>> s_decoders;

        static SC3String()
        {
            s_encoders = new Dictionary<SC3Game, Lazy<SC3StringEncoder>>();
            s_decoders = new Dictionary<SC3Game, Lazy<SC3StringDecoder>>();
            foreach (var game in GameSpecificData.KnownGames)
            {
                s_encoders[game] = new Lazy<SC3StringEncoder>(() => new SC3StringEncoder(game));
                s_decoders[game] = new Lazy<SC3StringDecoder>(() => new SC3StringDecoder(game));
            }
        }

        public SC3String(ImmutableArray<SC3StringSegment> segments, bool isProperlyTerminated)
        {
            Segments = segments;
            IsProperlyTerminated = isProperlyTerminated;
        }

        public ImmutableArray<SC3StringSegment> Segments { get; }
        public bool IsProperlyTerminated { get; }

        public static SC3String FromBytes(byte[] bytes, SC3Game game)
        {
            var decoder = s_decoders[game].Value;
            return decoder.DecodeString(bytes);
        }

        public static SC3String Deserialize(string text)
        {
            return DefaultSC3StringDeserializer.Instance.Deserialize(text);
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
        /// Returns the dialogue part of this <see cref="SC3String"/> (everything after [name]).
        /// If no character name is specified, the whole string is returned.
        /// </summary>
        public SC3String GetDialogueLine()
        {
            if (Segments.Length == 0)
            {
                return SC3String.Empty;
            }

            if (!(Segments[0] is CharacterNameStartCommand))
            {
                return this;
            }

            int idxNameSegment = -1;
            for (int i = 0; i < Segments.Length; i++)
            {
                if ((Segments[i] as EmbeddedCommand)?.CommandKind == EmbeddedCommandKind.DialogueLineStart)
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
            return new SC3String(segments.ToImmutableArray(), false);
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

            if (!(Segments[0] is CharacterNameStartCommand))
            {
                return SC3String.Empty;
            }

            var segments = Segments.Skip(1).TakeWhile(s => (s as EmbeddedCommand)?.CommandKind != EmbeddedCommandKind.DialogueLineStart);
            return new SC3String(segments.ToImmutableArray(), false);
        }

        public override string ToString()
        {
            return DefaultSC3StringSerializer.Instance.Serialize(this);
        }

        public bool Equals(SC3String other)
        {
            return true;
        }

        public bool Equals(SC3String other, bool ignoreWidth)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            if (Segments.Length != other.Segments.Length)
            {
                return false;
            }

            for (int i = 0; i < Segments.Length; i++)
            {
                var segA = Segments[i];
                var segB = other.Segments[i];

                if (segA.SegmentKind != segB.SegmentKind)
                {
                    return false;
                }

                if (segA is TextSegment textSegA && segB is TextSegment textSegB)
                {
                    if (!textSegA.Equals(textSegB, ignoreWidth))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!segA.Equals(segB))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public abstract class SC3StringSegment
    {
        public abstract SC3StringSegmentKind SegmentKind { get; }

        internal abstract void Accept(SC3StringSegmentVisitor visitor);

        public override string ToString()
        {
            return DefaultSC3StringSerializer.Instance.SerializeSegment(this);
        }
    }

    public enum SC3StringSegmentKind
    {
        Text,
        EmbeddedCommand
    }

    public sealed class TextSegment : SC3StringSegment, IEquatable<TextSegment>
    {
        public TextSegment(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override SC3StringSegmentKind SegmentKind => SC3StringSegmentKind.Text;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitTextSegment(this);
        }

        public bool Equals(TextSegment other) => ReferenceEquals(this, other) || Value.Equals(other?.Value);
        public bool Equals(TextSegment other, bool ignoreWidth)
        {
            var options = ignoreWidth ? CompareOptions.IgnoreWidth : CompareOptions.Ordinal;
            return ReferenceEquals(this, other)
                || string.Compare(Value, other?.Value, CultureInfo.InvariantCulture, options) == 0;
        }

        public override bool Equals(object obj) => Equals(obj as TextSegment);
        public override int GetHashCode() => -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
    }

    public abstract class EmbeddedCommand : SC3StringSegment
    {
        public override SC3StringSegmentKind SegmentKind => SC3StringSegmentKind.EmbeddedCommand;
        public abstract EmbeddedCommandKind CommandKind { get; }
    }

    public enum EmbeddedCommandKind
    {
        LineBreak,
        CharacterNameStart,
        DialogueLineStart,
        Present,
        SetColor,
        RubyBaseStart,
        RubyTextStart,
        RubyTextEnd,
        SetFontSize,
        PrintInParallel,
        CenterText,
        SetMargin,
        GetHardcodedValue,
        EvaluateExpression,
        AutoForward,
        Unknown,
        RubyCenterPerChar
    }

    public sealed class LineBreakCommand : EmbeddedCommand, IEquatable<LineBreakCommand>
    {
        public LineBreakCommand(byte code)
        {
            Code = code;
        }

        // Either 0x00 or 0x1F
        public byte Code { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.LineBreak;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitLineBreakCommand(this);
        }

        public bool Equals(LineBreakCommand other) => ReferenceEquals(this, other) || Code == other?.Code;
        public override bool Equals(object obj) => Equals(obj as LineBreakCommand);
        public override int GetHashCode() => -434485196 + Code.GetHashCode();
    }

    public sealed class CharacterNameStartCommand : EmbeddedCommand, IEquatable<CharacterNameStartCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.CharacterNameStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitCharacterNameStartCommand(this);
        }

        public bool Equals(CharacterNameStartCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => 1237277532 + CommandKind.GetHashCode();
    }

    public sealed class DialogueLineStartCommand : EmbeddedCommand, IEquatable<DialogueLineStartCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.DialogueLineStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitDialogueLineStartCommand(this);
        }

        public bool Equals(DialogueLineStartCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class PresentCommand : EmbeddedCommand, IEquatable<PresentCommand>
    {
        public enum SideEffectKind
        {
            None,
            ResetTextAlignment,
            Unknown_0x18
        }

        internal PresentCommand(PresentCommand.SideEffectKind attachedAction)
        {
            SideEffect = attachedAction;
        }

        public PresentCommand.SideEffectKind SideEffect { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.Present;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitPresentCommand(this);
        }

        public bool Equals(PresentCommand other) => ReferenceEquals(this, other) || SideEffect == other?.SideEffect;
        public override bool Equals(object obj) => Equals(obj as PresentCommand);
        public override int GetHashCode() => 499025483 + SideEffect.GetHashCode();
    }

    public sealed class SetColorCommand : EmbeddedCommand, IEquatable<SetColorCommand>
    {
        public SetColorCommand(Expression colorIndex)
        {
            ColorIndex = colorIndex;
        }

        public Expression ColorIndex { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.SetColor;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitSetColorCommand(this);
        }

        public bool Equals(SetColorCommand other)
        {
            return ReferenceEquals(this, other) || ColorIndex.Equals(other?.ColorIndex);
        }

        public override bool Equals(object obj) => Equals(obj as SetColorCommand);
        public override int GetHashCode() => 1516799092 + EqualityComparer<Expression>.Default.GetHashCode(ColorIndex);
    }

    public sealed class RubyBaseStartCommand : EmbeddedCommand, IEquatable<RubyBaseStartCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyBaseStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyBaseStartCommand(this);
        }

        public bool Equals(RubyBaseStartCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class RubyTextStartCommand : EmbeddedCommand, IEquatable<RubyTextStartCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyTextStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyTextStartCommand(this);
        }

        public bool Equals(RubyTextStartCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class RubyTextEndCommand : EmbeddedCommand, IEquatable<RubyTextEndCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyTextEnd;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyTextEndCommand(this);
        }

        public bool Equals(RubyTextEndCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class SetFontSizeCommand : EmbeddedCommand, IEquatable<SetFontSizeCommand>
    {
        public SetFontSizeCommand(int fontSize)
        {
            FontSize = fontSize;
        }
        
        public int FontSize { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.SetFontSize;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitSetFontSizeCommand(this);
        }

        public bool Equals(SetFontSizeCommand other) => ReferenceEquals(this, other) || FontSize == other?.FontSize;
        public override bool Equals(object obj) => Equals(obj as SetFontSizeCommand);
        public override int GetHashCode() => 798539017 + FontSize.GetHashCode();
    }

    public sealed class PrintInParallelCommand : EmbeddedCommand, IEquatable<PrintInParallelCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.PrintInParallel;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitPrintInParallelCommand(this);
        }

        public bool Equals(PrintInParallelCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class CenterTextCommand : EmbeddedCommand, IEquatable<CenterTextCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.CenterText;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitCenterTextCommand(this);
        }

        public bool Equals(CenterTextCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }

    public sealed class SetMarginCommand : EmbeddedCommand, IEquatable<SetMarginCommand>
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

        public bool Equals(SetMarginCommand other)
        {
            return ReferenceEquals(this, other) || LeftMargin == other?.LeftMargin && TopMargin == other?.TopMargin;
        }

        public override bool Equals(object obj) => Equals(obj as SetMarginCommand);
        public override int GetHashCode()
        {
            var hashCode = 1916113840;
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(LeftMargin);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(TopMargin);
            return hashCode;
        }
    }

    public sealed class GetHardcodedValueCommand : EmbeddedCommand, IEquatable<GetHardcodedValueCommand>
    {
        public GetHardcodedValueCommand(int index)
        {
            Index = index;
        }

        public int Index { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.GetHardcodedValue;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitGetHardcodedValueCommand(this);
        }

        public bool Equals(GetHardcodedValueCommand other) => ReferenceEquals(this, other) || Index == other?.Index;
        public override bool Equals(object obj) => Equals(obj as GetHardcodedValueCommand);
        public override int GetHashCode() => -2134847229 + Index.GetHashCode();
    }

    public sealed class EvaluateExpressionCommand : EmbeddedCommand, IEquatable<EvaluateExpressionCommand>
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

        public bool Equals(EvaluateExpressionCommand other)
        {
            return ReferenceEquals(this, other) || Expression.Equals(other?.Expression);
        }

        public override bool Equals(object obj) => Equals(obj as EvaluateExpressionCommand);
        public override int GetHashCode() => EqualityComparer<Expression>.Default.GetHashCode(Expression);
    }

    public sealed class AutoForwardCommand : EmbeddedCommand, IEquatable<AutoForwardCommand>
    {
        public AutoForwardCommand(byte code)
        {
            Code = code;
        }

        // Either 0x19 or 0x1A
        public byte Code { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.AutoForward;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitAutoForwardCommand(this);
        }

        public bool Equals(AutoForwardCommand other) => ReferenceEquals(this, other) || Code == other?.Code;
        public override bool Equals(object obj) => Equals(obj as AutoForwardCommand);
        public override int GetHashCode() => -434485196 + Code.GetHashCode();
    }

    public sealed class RubyCenterPerCharCommand : EmbeddedCommand, IEquatable<RubyCenterPerCharCommand>
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyCenterPerChar;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyCenterPerCharCommand(this);
        }

        public bool Equals(RubyCenterPerCharCommand other) => other != null;
        public override bool Equals(object obj) => obj != null;
        public override int GetHashCode() => -1237277532 + CommandKind.GetHashCode();
    }
}

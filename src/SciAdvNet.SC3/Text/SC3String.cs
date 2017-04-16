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

        /// <summary>
        /// The collection of segments that this instance of <see cref="SC3String"/> consists of. 
        /// </summary>
        public ImmutableArray<SC3StringSegment> Segments { get; }

        public bool IsProperlyTerminated { get; }

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
        /// Extracts the dialogue line from this <see cref="SC3String"/>.
        /// Returns the whole string if the character name is not specified.
        /// </summary>
        public SC3String GetDialogueLine()
        {
            if (Segments.Length == 0)
            {
                return SC3String.Empty;
            }

            var command = Segments[0] as EmbeddedCommand;
            if (command == null || command.CommandKind != EmbeddedCommandKind.CharacterNameStart)
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

            var command = Segments[0] as EmbeddedCommand;
            if (command == null || command.CommandKind != EmbeddedCommandKind.CharacterNameStart)
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
    }

    public abstract class SC3StringSegment
    {
        public abstract SC3StringSegmentKind SegmentKind { get; }

        internal abstract void Accept(SC3StringSegmentVisitor visitor);

        public override String ToString()
        {
            return DefaultSC3StringSerializer.Instance.SerializeSegment(this);
        }
    }

    public enum SC3StringSegmentKind
    {
        Text,
        EmbeddedCommand
    }

    public sealed class TextSegment : SC3StringSegment
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
        Unknown
    }

    public sealed class LineBreakCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.LineBreak;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitLineBreakCommand(this);
        }
    }

    public sealed class CharacterNameStartCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.CharacterNameStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitCharacterNameStartCommand(this);
        }
    }

    public sealed class DialogueLineStartCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.DialogueLineStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitDialogueLineStartCommand(this);
        }
    }

    public sealed class PresentCommand : EmbeddedCommand
    {
        public enum Action
        {
            None,
            ResetTextAlignment,
            Unknown_0x18
        }

        internal PresentCommand(PresentCommand.Action attachedAction)
        {
            AttachedAction = attachedAction;
        }

        public PresentCommand.Action AttachedAction { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.Present;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitPresentCommand(this);
        }
    }

    public sealed class SetColorCommand : EmbeddedCommand
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
    }

    public sealed class RubyBaseStartCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyBaseStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyBaseStartCommand(this);
        }
    }

    public sealed class RubyTextStartCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyTextStart;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyTextStartCommand(this);
        }
    }

    public sealed class RubyTextEndCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.RubyTextEnd;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitRubyTextEndCommand(this);
        }
    }

    public sealed class SetFontSizeCommand : EmbeddedCommand
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
    }

    public sealed class PrintInParallelCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.PrintInParallel;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitPrintInParallelCommand(this);
        }
    }

    public sealed class CenterTextCommand : EmbeddedCommand
    {
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

    public sealed class GetHardcodedValueCommand : EmbeddedCommand
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

    public sealed class AutoForwardCommand : EmbeddedCommand
    {
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.AutoForward;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitAutoForwardCommand(this);
        }
    }

    public sealed class UnknownCommand : EmbeddedCommand
    {
        public UnknownCommand(ImmutableArray<byte> bytes)
        {
            Bytes = bytes;
        }

        public ImmutableArray<byte> Bytes { get; }
        public override EmbeddedCommandKind CommandKind => EmbeddedCommandKind.Unknown;

        internal override void Accept(SC3StringSegmentVisitor visitor)
        {
            visitor.VisitUnknownCommand(this);
        }
    }
}

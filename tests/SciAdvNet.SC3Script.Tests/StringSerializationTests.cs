using SciAdvNet.SC3Script.Text;
using System;
using System.Linq;
using Xunit;

namespace SciAdvNet.SC3Script.Tests
{
    public sealed class StringSerializationTests
    {
        [Fact]
        public void DeserializeTextInBrackets()
        {
            string text = "[Fictional]";
            var sc3String = SC3String.Deserialize(text);

            var segment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(segment);
            Assert.Equal(SC3StringSegmentKind.Text, segment.SegmentKind);
            Assert.Equal(text, segment.Value);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeLineBreak()
        {
            TestTag("[linebreak]", EmbeddedCommandKind.LineBreak);
        }

        [Fact]
        public void DeserializeAltLineBreak()
        {
            TestTag("[alt-linebreak]", EmbeddedCommandKind.LineBreak);
        }

        [Fact]
        public void DeserializeNameTag()
        {
            TestTag("[name]", EmbeddedCommandKind.CharacterNameStart);
        }

        [Fact]
        public void DeserializeLineTag()
        {
            TestTag("[line]", EmbeddedCommandKind.DialogueLineStart);
        }

        [Fact]
        public void DeserializePresentTag()
        {
            var command = TestTag("[%p]", EmbeddedCommandKind.Present) as PresentCommand;
            Assert.Equal(PresentCommand.SideEffectKind.None, command.SideEffect);
        }

        [Fact]
        public void DeserializeColorTag()
        {
            var command = TestTag("[color index=\"820000\"]", EmbeddedCommandKind.SetColor) as SetColorCommand;
            Assert.NotNull(command.ColorIndex);

        }

        [Fact]
        public void DeserializePresentTag_ResetAlignment()
        {
            var command = TestTag("[%e]", EmbeddedCommandKind.Present) as PresentCommand;
            Assert.Equal(PresentCommand.SideEffectKind.ResetTextAlignment, command.SideEffect);
        }

        [Fact]
        public void DeserializeRubyBaseTag()
        {
            TestTag("[ruby-base]", EmbeddedCommandKind.RubyBaseStart);
        }

        [Fact]
        public void DeserializeRubyTextStartTag()
        {
            TestTag("[ruby-text-start]", EmbeddedCommandKind.RubyTextStart);
        }

        [Fact]
        public void DeserializeRubyTextStartEnd()
        {
            TestTag("[ruby-text-end]", EmbeddedCommandKind.RubyTextEnd);
        }

        [Fact]
        public void DeserializeFontSizeTag()
        {
            var command = TestTag("[font size=\"42\"]", EmbeddedCommandKind.SetFontSize) as SetFontSizeCommand;
            Assert.Equal(42, command.FontSize);
        }

        [Fact]
        public void DeserializeParallelTag()
        {
            TestTag("[parallel]", EmbeddedCommandKind.PrintInParallel);
        }

        [Fact]
        public void DeserializeCenterTag()
        {
            TestTag("[center]", EmbeddedCommandKind.CenterText);
        }

        [Fact]
        public void DeserializeLeftMarginTag()
        {
            var command = TestTag("[margin left=\"42\"]", EmbeddedCommandKind.SetMargin) as SetMarginCommand;
            Assert.Equal(42, command.LeftMargin);
            Assert.False(command.TopMargin.HasValue);
        }

        [Fact]
        public void DeserializeHardcodedValueTag()
        {
            var command = TestTag("[hardcoded-value index=\"1\"]", EmbeddedCommandKind.GetHardcodedValue) as GetHardcodedValueCommand;
            Assert.Equal(1, command.Index);
        }

        [Fact]
        public void DeserializeTopMarginTag()
        {
            var command = TestTag("[margin top=\"42\"]", EmbeddedCommandKind.SetMargin) as SetMarginCommand;
            Assert.Equal(42, command.TopMargin);
            Assert.False(command.LeftMargin.HasValue);
        }

        [Fact]
        public void DeserializeAutoForwardTag()
        {
            TestTag("[auto-forward]", EmbeddedCommandKind.AutoForward);
        }

        [Fact]
        public void DeserializeAltAutoForwardTag()
        {
            TestTag("[auto-forward-1a]", EmbeddedCommandKind.AutoForward);
        }

        [Fact]
        public void DeserializeEvaluateExpressionTag()
        {
            var command = TestTag("[evaluate expr=\"290AA4B5141400810000\"]", EmbeddedCommandKind.EvaluateExpression) as EvaluateExpressionCommand;
            Assert.NotNull(command);
            Assert.Equal(Utils.HexStringToBytes("290AA4B5141400810000"), command.Expression.Bytes);
        }

        private EmbeddedCommand TestTag(string text, EmbeddedCommandKind expectedCommandKind)
        {
            var sc3String = SC3String.Deserialize(text);
            var command = sc3String.Segments[0] as EmbeddedCommand;
            Assert.NotNull(command);
            Assert.Equal(expectedCommandKind, command.CommandKind);
            Assert.Equal(text, sc3String.ToString());

            return command;
        }
    }
}

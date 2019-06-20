using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace SciAdvNet.SC3Script.Tests
{
    public sealed class StringDecodingTests
    {
        [Fact]
        public void DecodeTextSegment()
        {
            string hex = "80C480C480C4";
            var sc3String = SC3String.FromBytes(Utils.HexStringToBytes(hex), SC3Game.SteinsGateZero);
            Assert.Single(sc3String.Segments);

            var textSegment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(textSegment);
            Assert.Equal(SC3StringSegmentKind.Text, textSegment.SegmentKind);
            Assert.NotNull(textSegment.Value);
        }

        [Fact]
        public void DecodeTextInBrackets()
        {
            // [Fictional]
            string hex = "805d8010802d80278038802d8033803280258030805fff";
            var sc3String = SC3String.FromBytes(Utils.HexStringToBytes(hex), SC3Game.SteinsGateZero);
            Assert.Single(sc3String.Segments);

            var textSegment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(textSegment);
            Assert.Equal("[Fictional]", textSegment.Value);

            Compare(hex, sc3String.Encode(SC3Game.SteinsGateZero));
        }

        [Fact]
        public void DecodeCompountCharacter()
        {
            string hex = "9EC1"; // [...]
            var sc3String = SC3String.FromBytes(Utils.HexStringToBytes(hex), SC3Game.ChaosChild);
            Assert.Single(sc3String.Segments);

            var textSegment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(textSegment);
            Assert.Equal("[...]", textSegment.Value);

            Compare(hex, sc3String.Encode(SC3Game.ChaosChild));
        }

        [Fact]
        public void DecodeLineBreakCommand()
        {
            var command = TestCommand("00", EmbeddedCommandKind.LineBreak) as LineBreakCommand;
            Assert.Equal(0x00, command.Code);
        }

        [Fact]
        public void DecodeAltLineBreakCommand()
        {
            var command = TestCommand("1F", EmbeddedCommandKind.LineBreak) as LineBreakCommand;
            Assert.Equal(0x1F, command.Code);
        }

        [Fact]
        public void DecodeCharacterNameStartCommand()
        {
            TestCommand("01", EmbeddedCommandKind.CharacterNameStart);
        }

        [Fact]
        public void DecodeDialogueLineStartCommand()
        {
            TestCommand("02", EmbeddedCommandKind.DialogueLineStart);
        }

        [Fact]
        public void DecodePresentCommand()
        {
            var command = TestCommand("03", EmbeddedCommandKind.Present) as PresentCommand;
            Assert.Equal(PresentCommand.SideEffectKind.None, command.SideEffect);
        }

        [Fact]
        public void DecodeSetColorCommand()
        {
            var command = TestCommand("04820000", EmbeddedCommandKind.SetColor) as SetColorCommand;
            Assert.NotNull(command.ColorIndex);
        }

        [Fact]
        public void DecodePresentCommand_ResetAlignment()
        {
            var command = TestCommand("08", EmbeddedCommandKind.Present) as PresentCommand;
            Assert.Equal(PresentCommand.SideEffectKind.ResetTextAlignment, command.SideEffect);
        }

        [Fact]
        public void DecodeRubyBaseStartCommand()
        {
            TestCommand("09", EmbeddedCommandKind.RubyBaseStart);
        }

        [Fact]
        public void DecodeRubyTextStartCommand()
        {
            TestCommand("0A", EmbeddedCommandKind.RubyTextStart);
        }

        [Fact]
        public void DecodeRubyTextEndCommand()
        {
            TestCommand("0B", EmbeddedCommandKind.RubyTextEnd);
        }

        [Fact]
        public void DecodeSetFontSizeCommand()
        {
            var command = TestCommand("0C0001", EmbeddedCommandKind.SetFontSize) as SetFontSizeCommand;
            Assert.Equal(1, command.FontSize);
        }

        [Fact]
        public void DecodePrintInParallelCommand()
        {
            TestCommand("0E", EmbeddedCommandKind.PrintInParallel);
        }

        [Fact]
        public void DecodeCenterTextCommand()
        {
            TestCommand("0F", EmbeddedCommandKind.CenterText);
        }

        [Fact]
        public void DecodeSetTopMarginCommand()
        {
            var command = TestCommand("110001", EmbeddedCommandKind.SetMargin) as SetMarginCommand;
            Assert.True(command.TopMargin.HasValue);
            Assert.False(command.LeftMargin.HasValue);

            Assert.Equal(1, command.TopMargin);
        }

        [Fact]
        public void DecodeSetLeftMarginCommand()
        {
            var command = TestCommand("120001", EmbeddedCommandKind.SetMargin) as SetMarginCommand;
            Assert.True(command.LeftMargin.HasValue);
            Assert.False(command.TopMargin.HasValue);

            Assert.Equal(1, command.LeftMargin);
        }

        [Fact]
        public void DecodeGetHardcodedValueCommand()
        {
            var command = TestCommand("130001", EmbeddedCommandKind.GetHardcodedValue) as GetHardcodedValueCommand;
            Assert.Equal(1, command.Index);
        }

        [Fact]
        public void DecodeAutoForwardCommand()
        {
            var command = TestCommand("19", EmbeddedCommandKind.AutoForward) as AutoForwardCommand;
            Assert.Equal(0x19, command.Code);
        }

        [Fact]
        public void DecodeAltAutoForwardCommand()
        {
            var command = TestCommand("1A", EmbeddedCommandKind.AutoForward) as AutoForwardCommand;
            Assert.Equal(0x1A, command.Code);
        }

        [Fact]
        public void DecodeRubyCenterPerCharCommand()
        {
            var command = TestCommand("1E", EmbeddedCommandKind.RubyCenterPerChar) as RubyCenterPerCharCommand;
        }

        private EmbeddedCommand TestCommand(string hex, EmbeddedCommandKind expectedCommandKind)
        {
            var sc3String = SC3String.FromBytes(Utils.HexStringToBytes(hex), SC3Game.SteinsGateZero);
            Assert.Single(sc3String.Segments);

            var command = sc3String.Segments[0] as EmbeddedCommand;
            Assert.NotNull(command);
            Assert.Equal(SC3StringSegmentKind.EmbeddedCommand, command.SegmentKind);
            Assert.Equal(expectedCommandKind, command.CommandKind);

            Compare(hex, sc3String.Encode(SC3Game.SteinsGateZero));

            return command;
        }

        private void Compare(string expectedHex, ImmutableArray<byte> actualBytes)
        {
            var expectedBytes = HexStrToBytes(expectedHex);
            Assert.Equal(expectedBytes, actualBytes.ToArray());
        }

        private static byte[] HexStrToBytes(string hexString)
        {
            hexString = CleanHexString(hexString);

            return Enumerable.Range(0, hexString.Length / 2)
                .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
                .ToArray();
        }

        private static string CleanHexString(string hexString)
        {
            return hexString.Replace("0x", string.Empty).Replace(" ", string.Empty);
        }
    }
}

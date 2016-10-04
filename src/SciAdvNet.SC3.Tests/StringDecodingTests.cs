using SciAdvNet.SC3.Text;
using Xunit;

namespace SciAdvNet.SC3.Tests
{
    public sealed class StringDecodingTests
    {
        [Fact]
        public void TestDecodingText()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeTextSegment(game);
            }
        }

        [Fact]
        public void TestDecodingLineBreak()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeLineBreak(game);
            }
        }


        [Fact]
        public void TestDecodingCharacterNameMarker()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeCharacterNameMarker(game);
            }
        }


        [Fact]
        public void TestDecodingDialogueLine()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeDialogueLineMarker(game);
            }
        }

        [Fact]
        public void TestDecodingPresentCommand()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodePresentCommand(game);
                DecodePresentCommand_ResetAlignment(game);
            }
        }

        [Fact]
        public void TestDecodingColoredText()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeColoredText(game);
            }
        }

        [Fact]
        public void TestDecodingRubyText()
        {
            foreach (var game in GameSpecificData.SupportedGames)
            {
                DecodeRubyBaseMarker(game);
                DecodeRubyTextStartMarker(game);
                DecodeRubyTextEndMarker(game);
            }
        }

        private void DecodeTextSegment(SC3Game game)
        {
            string hex = "80C480C480C4";
            var sc3String = SC3String.FromHexString(hex, game);

            Assert.Equal(1, sc3String.Segments.Length);
            var textSegment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(textSegment);
            Assert.Equal(SC3StringSegmentKind.Text, textSegment.SegmentKind);
            Assert.NotNull(textSegment.Value);
        }

        private void DecodeLineBreak(SC3Game game)
        {
            string hex = "00";
            var sc3String = SC3String.FromHexString(hex, game);

            Assert.Equal(1, sc3String.Segments.Length);

            var textSegment = sc3String.Segments[0] as TextSegment;
            Assert.NotNull(textSegment);
            Assert.Equal("\n", textSegment.Value);
        }

        private void DecodeCharacterNameMarker(SC3Game game)
        {
            string hex = "01";
            var sc3String = SC3String.FromHexString(hex, game);

            Assert.Equal(1, sc3String.Segments.Length);

            var marker = sc3String.Segments[0] as Marker;
            Assert.NotNull(marker);
            Assert.Equal(SC3StringSegmentKind.Marker, marker.SegmentKind);
            Assert.Equal(MarkerKind.CharacterName, marker.MarkerKind);
        }

        private void DecodeDialogueLineMarker(SC3Game game)
        {
            string hex = "02";
            var sc3String = SC3String.FromHexString(hex, game);

            Assert.Equal(1, sc3String.Segments.Length);

            var marker = sc3String.Segments[0] as Marker;
            Assert.NotNull(marker);
            Assert.Equal(SC3StringSegmentKind.Marker, marker.SegmentKind);
            Assert.Equal(MarkerKind.DialogueLine, marker.MarkerKind);
        }

        private void DecodeRubyBaseMarker(SC3Game game)
        {
            string hex = "09";
            var sc3String = SC3String.FromHexString(hex, game);
            Assert.Equal(1, sc3String.Segments.Length);

            var rubyBaseMarker = sc3String.Segments[0] as Marker;
            Assert.NotNull(rubyBaseMarker);
            Assert.Equal(SC3StringSegmentKind.Marker, rubyBaseMarker.SegmentKind);
            Assert.Equal(MarkerKind.RubyBase, rubyBaseMarker.MarkerKind);
        }

        private void DecodeRubyTextStartMarker(SC3Game game)
        {
            string hex = "0A";
            var sc3String = SC3String.FromHexString(hex, game);
            Assert.Equal(1, sc3String.Segments.Length);

            var rubyTextStartMarker = sc3String.Segments[0] as Marker;
            Assert.NotNull(rubyTextStartMarker);
            Assert.Equal(SC3StringSegmentKind.Marker, rubyTextStartMarker.SegmentKind);
            Assert.Equal(MarkerKind.RubyTextStart, rubyTextStartMarker.MarkerKind);
        }

        private void DecodeRubyTextEndMarker(SC3Game game)
        {
            string hex = "0B";
            var sc3String = SC3String.FromHexString(hex, game);
            Assert.Equal(1, sc3String.Segments.Length);

            var rubyTextEndMarker = sc3String.Segments[0] as Marker;
            Assert.NotNull(rubyTextEndMarker);
            Assert.Equal(SC3StringSegmentKind.Marker, rubyTextEndMarker.SegmentKind);
            Assert.Equal(MarkerKind.RubyTextEnd, rubyTextEndMarker.MarkerKind);
        }

        private void DecodePresentCommand(SC3Game game)
        {
            var sc3String = SC3String.FromHexString("03FF", game);
            Assert.Equal(1, sc3String.Segments.Length);

            var command = sc3String.Segments[0] as PresentCommand;
            Assert.NotNull(command);
            Assert.Equal(SC3StringSegmentKind.EmbeddedCommand, command.SegmentKind);
            Assert.Equal(EmbeddedCommandKind.Present, command.CommandKind);
            Assert.Equal(false, command.ResetTextAlignment);
        }

        private void DecodePresentCommand_ResetAlignment(SC3Game game)
        {
            var sc3String = SC3String.FromHexString("08FF", game);
            Assert.Equal(1, sc3String.Segments.Length);

            var command = sc3String.Segments[0] as PresentCommand;
            Assert.NotNull(command);
            Assert.Equal(SC3StringSegmentKind.EmbeddedCommand, command.SegmentKind);
            Assert.Equal(EmbeddedCommandKind.Present, command.CommandKind);
            Assert.Equal(true, command.ResetTextAlignment);
        }

        private void DecodeColoredText(SC3Game game)
        {
            string hex = "04 820000 809D80AB80A8803F809880B580AA80A480B180AC80BD80A480B780AC80B280B1";
            var sc3String = SC3String.FromHexString(hex, game);

            Assert.Equal(2, sc3String.Segments.Length);

            var setColorCommand = sc3String.Segments[0] as SetColorCommand;
            var textSegment = sc3String.Segments[1] as TextSegment;

            Assert.NotNull(setColorCommand);
            Assert.NotNull(textSegment);

            Assert.Equal(SC3StringSegmentKind.EmbeddedCommand, setColorCommand.SegmentKind);
            Assert.Equal(EmbeddedCommandKind.SetColor, setColorCommand.CommandKind);
        }
    }
}

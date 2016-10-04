using SciAdvNet.SC3.Text;
using System.Diagnostics;
using Xunit;

namespace SciAdvNet.SC3.Tests
{
    public sealed class StringDeserializationTests
    {
        [Fact]
        public void DeserializeColorTag()
        {
            string text = "[color index=\"820000\"]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as SetColorCommand;
            Assert.NotNull(command);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeLeftMarginTag()
        {
            string text = "[margin left=\"42\"]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as SetMarginCommand;
            Assert.NotNull(command);
            Assert.Equal(42, command.LeftMargin);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeTopMarginTag()
        {
            string text = "[margin top=\"42\"]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as SetMarginCommand;
            Assert.NotNull(command);
            Assert.Equal(42, command.TopMargin);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeFontSizeTag()
        {
            string text = "[font size=\"42\"]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as SetFontSizeCommand;
            Assert.NotNull(command);
            Assert.Equal(42, command.FontSize);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeCenterTag()
        {
            string text = "[center]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as CenterTextCommand;
            Assert.NotNull(command);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializePresentTag_ResetAlignment()
        {
            string text = "[%e]";
            var sc3String = SC3String.Deserialize(text);

            var command = sc3String.Segments[0] as PresentCommand;
            Assert.NotNull(command);
            Assert.True(command.ResetTextAlignment);
            Assert.Equal(text, sc3String.ToString());
        }

        [Fact]
        public void DeserializeMarkers()
        {
            TestMarker("[name]", MarkerKind.CharacterName);
            TestMarker("[line]", MarkerKind.DialogueLine);
            TestMarker("[rubyBase]", MarkerKind.RubyBase);
            TestMarker("[rubyTextStart]", MarkerKind.RubyTextStart);
            TestMarker("[rubyTextEnd]", MarkerKind.RubyTextEnd);
        }

        private void TestMarker(string text, MarkerKind expectedKind)
        {
            var sc3String = SC3String.Deserialize(text);
            var marker = sc3String.Segments[0] as Marker;
            Assert.NotNull(marker);
            Assert.Equal(expectedKind, marker.MarkerKind);
            Assert.Equal(text, sc3String.ToString());
        }
    }
}

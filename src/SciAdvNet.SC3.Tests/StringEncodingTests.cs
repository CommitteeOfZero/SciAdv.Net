using SciAdvNet.SC3.Text;
using System.Linq;
using Xunit;

namespace SciAdvNet.SC3.Tests
{
    public sealed class StringEncodingTests
    {
        [Fact]
        public void EncodeLatinText()
        {
            string text = "fuckety fuck";
            var sc3String = SC3String.Deserialize(text);

            var bytes = sc3String.Encode(SC3Game.SteinsGateHD);
            string s = SC3String.FromBytes(bytes.ToArray(), SC3Game.SteinsGateHD).ToString();

        }

        [Fact]
        public void EncodeCharacterNameMarker()
        {
            string text = "[name]";
            var sc3String = SC3String.Deserialize(text);

            var encoded = sc3String.Encode(SC3Game.SteinsGateHD);
            Assert.Equal(0x01, encoded[0]);
        }

        [Fact]
        public void EncodeDialogueLineMarker()
        {
            string text = "[line]";
            var sc3String = SC3String.Deserialize(text);

            var encoded = sc3String.Encode(SC3Game.SteinsGateHD);
            Assert.Equal(0x02, encoded[0]);
        }

        [Fact]
        public void EncodeRubyBaseMarker()
        {
            string text = "[rubyBase]";
            var sc3String = SC3String.Deserialize(text);

            var encoded = sc3String.Encode(SC3Game.SteinsGateHD);
            Assert.Equal(0x09, encoded[0]);
        }

        [Fact]
        public void EncodeRubyTextStartMarker()
        {
            string text = "[rubyTextStart]";
            var sc3String = SC3String.Deserialize(text);

            var encoded = sc3String.Encode(SC3Game.SteinsGateHD);
            Assert.Equal(0x0A, encoded[0]);
        }

        [Fact]
        public void EncodeRubyTextEndMarker()
        {
            string text = "[rubyTextEnd]";
            var sc3String = SC3String.Deserialize(text);

            var encoded = sc3String.Encode(SC3Game.SteinsGateHD);
            Assert.Equal(0x0B, encoded[0]);
        }
    }
}

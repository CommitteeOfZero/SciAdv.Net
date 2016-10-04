using SciAdvNet.SC3.Text;

namespace SC3Enc
{
    /// <summary>
    /// Deserializer that skips string indexes.
    /// </summary>
    public sealed class CustomizedDeserializer : DefaultSC3StringDeserializer
    {
        public override SC3StringSegment DeserializeTag(ParsedTag tag)
        {
            if (IsNumber(tag.Name))
            {
                return null;
            }

            if (tag.Name == "comment")
            {
                return null;
            }

            return base.DeserializeTag(tag);
        }

        private bool IsNumber(string s)
        {
            int n;
            return int.TryParse(s, out n);
        }
    }
}

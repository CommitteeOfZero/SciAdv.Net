using SciAdvNet.SC3.Text;

namespace ProjectCtrlF
{
    public sealed class CustomizedDeserializer : DefaultSC3StringDeserializer
    {
        public int? ScriptId { get; private set; }
        public int? StringId { get; private set; }

        public override SC3String Deserialize(string text)
        {
            ScriptId = null;
            StringId = null;
            return base.Deserialize(text);
        }

        public override SC3StringSegment DeserializeTag(MarkupTag tag)
        {
            if (IsNumber(tag.Name))
            {
                int n = int.Parse(tag.Name);
                if (ScriptId.HasValue)
                {
                    StringId = n;
                }
                else
                {
                    ScriptId = n;
                }

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

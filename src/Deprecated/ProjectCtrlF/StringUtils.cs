using System.Text;

namespace ProjectCtrlF
{
    public static class StringUtils
    {
        private const char WideBlockStart = (char)0xFEE0;
        private const char WideBlockEnd = (char)(0xFEE0 + 127);
        private const char NormalSpace = ' ';
        private const char FullwidthSpace = (char)0x3000;

        public static string NormalizeString(string s)
        {
            var updated = new StringBuilder(s);
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    updated[i] = NormalSpace;
                }
                else if (s[i] >= WideBlockStart && s[i] <= WideBlockEnd)
                {
                    updated[i] = (char)(s[i] - WideBlockStart);
                }
            }

            return updated.ToString();
        }

        public static string ConvertToFullwidth(string s)
        {
            var updated = new StringBuilder(s);
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    updated[i] = FullwidthSpace;
                }
                else if (s[i] < 128)
                {
                    updated[i] = (char)(s[i] + WideBlockStart);
                }
            }

            return updated.ToString();
        }
    }
}

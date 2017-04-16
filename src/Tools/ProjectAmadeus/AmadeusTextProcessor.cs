using System.Collections.Generic;
using System.Text;

namespace ProjectAmadeus
{
    public static class AmadeusTextProcessor
    {
        private const char WideBlockStart = (char)0xFEE0;
        private const char WideBlockEnd = (char)(0xFEE0 + 127);
        private const char NormalSpace = ' ';
        private const char FullwidthSpace = (char)0x3000;

        private const char RegularQtMark = '"';
        private const char LeftQtMark = '“';
        private const char RightQtMark = '”';

        private static readonly Dictionary<char, char> s_replacementTable = new Dictionary<char, char>()
        {
            ['\''] = '’',
            [NormalSpace] = FullwidthSpace
        };

        public static string Preprocess(string text)
        {
            var sb = new StringBuilder(text);
            int totalQtMarks = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                char replacement;

                if (s_replacementTable.TryGetValue(character, out replacement))
                {
                    sb[i] = replacement;
                    continue;
                }

                if (character == RegularQtMark)
                {
                    totalQtMarks++;
                    replacement = totalQtMarks % 2 != 0 ? LeftQtMark : RightQtMark;
                    sb[i] = replacement;
                    continue;
                }
            }

            return sb.ToString();
        }

        public static string Normalize(string s)
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
    }
}

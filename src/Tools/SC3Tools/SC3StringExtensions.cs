using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace SC3Tools
{
    public static class SC3StringExtensions
    {
        private static bool IsFullwidthLetter(char c) => char.IsLetter(c) && TextUtils.IsFullwidthCharacter(c);

        public static bool IsFullwidth(this SC3String sc3String)
        {
            return sc3String.Segments.OfType<TextSegment>().Any(x => x.Value.Any(IsFullwidthLetter));
        }

        public static SC3String ToFullwidthString(this SC3String sc3String)
        {
            return ProcessTextSegments(sc3String, TextUtils.ConvertToFullwidth);
        }

        public static SC3String UseWideSpaces(this SC3String sc3String)
        {
            return ProcessTextSegments(sc3String, s => s.Replace(' ', (char)0x3000));
        }

        public static SC3String ProcessTextSegments(this SC3String sc3String, Func<string, string> processingFunc)
        {
            var segments = ImmutableArray.CreateBuilder<SC3StringSegment>();
            foreach (var originalSegment in sc3String.Segments)
            {
                if (originalSegment is TextSegment textSegment)
                {
                    string processedText = processingFunc(textSegment.Value);
                    var modifiedSegment = new TextSegment(processedText);
                    segments.Add(modifiedSegment);
                }
                else
                {
                    segments.Add(originalSegment);
                }
            }

            return new SC3String(segments.ToImmutable(), sc3String.IsProperlyTerminated);
        }
    }
}

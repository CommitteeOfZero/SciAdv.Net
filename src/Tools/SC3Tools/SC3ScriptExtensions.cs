using SciAdvNet.SC3Script;
using SciAdvNet.SC3Script.Text;
using System.IO;

namespace SC3Tools
{
    public static class SC3ScriptExtensions
    {
        public static void ExtractStrings(this SC3Script script, string outputFilePath, string userCharacters, bool normalize)
        {
            var decoder = new SC3StringDecoder(script.Game, userCharacters);
            using (var writer = File.CreateText(outputFilePath))
            {
                foreach (var stringTableEntry in script.StringTable)
                {
                    try
                    {
                        string line = decoder.DecodeString(stringTableEntry.RawData).ToString();
                        if (normalize)
                        {
                            line = TextUtils.NormalizeString(line);
                        }
                        writer.WriteLine(line);
                    }
                    catch (StringDecodingFailedException e)
                    {
                        string message = $"{e.Message}\nat line {stringTableEntry.Id} (offset: 0x{stringTableEntry.Offset + e.Position:X8})";
                        throw new ExtractingStringsFailed(message);
                    }
                }
            }
        }

        public static void UpdateStringTable(this SC3Script script, string[] lines, string userCharacters)
        {
            if (lines.Length != script.StringTable.Count)
            {
                throw new StringReplacementFailed("The input text file should have exactly as many lines as the original script.");
            }

            var encoder = new SC3StringEncoder(script.Game, userCharacters);
            var decoder = new SC3StringDecoder(script.Game, userCharacters);
            for (int i = 0; i < lines.Length; i++)
            {
                SC3String originalSc3String;
                try
                {
                    originalSc3String = decoder.DecodeString(script.StringTable[i].RawData);
                }
                catch (StringDecodingFailedException e)
                {
                    throw new StringReplacementFailed(e.Message, e);
                }

                var updatedSc3String = SC3String.Deserialize(lines[i]);
                if (!updatedSc3String.Equals(originalSc3String, ignoreWidth: true))
                {
                    updatedSc3String = originalSc3String.IsFullwidth() ?
                        updatedSc3String.ToFullwidthString() : updatedSc3String.UseWideSpaces();

                    try
                    {
                        var bytes = encoder.Encode(updatedSc3String);
                        script.UpdateString(i, bytes);
                    }
                    catch (StringEncodingFailedException e)
                    {
                        string message = $"{e.Message}\nat line {i}.";
                        throw new StringReplacementFailed(message, e);
                    }
                }
            }
        }
    }
}

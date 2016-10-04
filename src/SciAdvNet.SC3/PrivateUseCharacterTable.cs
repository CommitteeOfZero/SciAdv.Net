using SciAdvNet.SC3.Utils;
using System.Collections.Immutable;
using System.IO;

namespace SciAdvNet.SC3
{
    internal static class PrivateUseCharacterTable
    {
        public static ImmutableDictionary<int, string> Parse(Stream tblFile)
        {
            using (var reader = new StreamReader(tblFile))
            {
                var builder = ImmutableDictionary.CreateBuilder<int, string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parts = line.Split('=');
                        string strIndex = parts[0].Substring(1, parts[0].Length - 2);
                        string value = parts[1];

                        parts = strIndex.Split('-');
                        if (parts.Length == 1)
                        {
                            int idx = BinaryUtils.HexStrToInt32(parts[0]);
                            builder[idx] = value;
                        }
                        else
                        {
                            int rangeStart = BinaryUtils.HexStrToInt32(parts[0]);
                            int rangeEnd = BinaryUtils.HexStrToInt32(parts[1]);
                            for (int i = rangeStart; i <= rangeEnd; i++)
                            {
                                builder[i] = value;
                            }
                        }
                    }
                }

                return builder.ToImmutable();
            }
        }
    }
}

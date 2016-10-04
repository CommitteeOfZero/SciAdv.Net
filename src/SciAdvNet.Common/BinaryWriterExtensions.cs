using System.IO;

namespace SciAdvNet.Common
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteNullTerminatedString(this BinaryWriter writer, string str)
        {
            writer.Write(str.ToCharArray());
            if (str[str.Length - 1] != '\0')
            {
                writer.Write((byte)0);
            }
        }
    }
}

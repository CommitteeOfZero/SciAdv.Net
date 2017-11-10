using System.IO;

namespace SciAdvNet.Vfs
{
    internal static class BinaryReaderExtensions
    {
        public static char[] PeekChars(this BinaryReader reader, int count)
        {
            var result = reader.ReadChars(count);
            reader.BaseStream.Position -= count;
            return result;
        }
    }
}

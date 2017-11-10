using System;
using System.Collections.Immutable;
using System.Linq;

namespace SciAdvNet.SC3Script.Utils
{
    internal static class BinaryUtils
    {
        public static byte[] HexStringToBytes(string hexString)
        {
            hexString = CleanHexString(hexString);

            return Enumerable.Range(0, hexString.Length / 2)
                .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
                .ToArray();
        }

        public static int HexStrToInt32(string hexString) => Convert.ToInt32(CleanHexString(hexString), 16);

        public static string BytesToHexString(byte[] bytes, string separator = "")
            => BitConverter.ToString(bytes).Replace("-", separator);

        public static string BytesToHexString(ImmutableArray<byte> bytes, string separator = "")
            => BitConverter.ToString(bytes.ToArray()).Replace("-", separator);

        public static int BytesToInt32(byte[] bytes, ByteOrder byteOrder = ByteOrder.LittleEndian)
            => BitConverter.ToInt32(ReverseIfBE(bytes, byteOrder), 0);

        private static string CleanHexString(string hexString)
        {
            return hexString.Replace("0x", string.Empty).Replace(" ", string.Empty);
        }

        private static byte[] ReverseIfBE(byte[] bytes, ByteOrder byteOrder)
            => byteOrder == ByteOrder.LittleEndian ? bytes : bytes.Reverse().ToArray();
    }


    public enum ByteOrder
    {
        LittleEndian = 0,
        BigEndian = 1
    }
}

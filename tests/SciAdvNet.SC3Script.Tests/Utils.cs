using System;
using System.Linq;

namespace SciAdvNet.SC3Script.Tests
{
    internal static class Utils
    {
        public static byte[] HexStringToBytes(string hexString)
        {
            return Enumerable.Range(0, hexString.Length / 2)
                .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
                .ToArray();
        }
    }
}

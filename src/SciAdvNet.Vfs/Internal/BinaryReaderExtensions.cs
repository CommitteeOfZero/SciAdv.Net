using System;
using System.IO;
using System.Text;

namespace SciAdvNet.Vfs
{
    internal static class BinaryReaderExtensions
    {
        public static byte[] ReadBytesReverse(this BinaryReader reader, int count)
        {
            var bytes = reader.ReadBytes(count);
            Array.Reverse(bytes);
            return bytes;
        }

        public static int ReadInt32BE(this BinaryReader reader)
        {
            return BitConverter.ToInt32(reader.ReadBytesReverse(4), 0);
        }

        public static uint ReadUInt32BE(this BinaryReader reader)
        {
            return BitConverter.ToUInt32(reader.ReadBytesReverse(4), 0);
        }

        public static long ReadInt64BE(this BinaryReader reader)
        {
            return BitConverter.ToInt64(reader.ReadBytesReverse(8), 0);
        }

        public static ulong ReadUInt64BE(this BinaryReader reader)
        {
            return BitConverter.ToUInt64(reader.ReadBytesReverse(8), 0);
        }

        public static short ReadInt16BE(this BinaryReader reader)
        {
            return BitConverter.ToInt16(reader.ReadBytesReverse(2), 0);
        }

        public static ushort ReadUInt16BE(this BinaryReader reader)
        {
            return BitConverter.ToUInt16(reader.ReadBytesReverse(2), 0);
        }

        public static byte PeekByte(this BinaryReader reader) => PeekByte(reader, 0);
        public static byte PeekByte(this BinaryReader reader, int offset)
        {
            reader.BaseStream.Position += offset;
            byte result = reader.ReadByte();
            reader.BaseStream.Position -= (offset + 1);
            return result;
        }

        public static byte[] PeekBytes(this BinaryReader reader, int count)
        {
            var result = reader.ReadBytes(count);
            reader.BaseStream.Position -= result.Length;
            return result;
        }

        public static int PeekInt32(this BinaryReader reader)
        {
            int result = reader.ReadInt32();
            reader.BaseStream.Position -= 4;
            return result;
        }

        public static char[] PeekChars(this BinaryReader reader, int count)
        {
            var result = reader.ReadChars(count);
            reader.BaseStream.Position -= count;
            return result;
        }

        public static string ReadNullTerminatedString(this BinaryReader reader, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                byte b;
                do
                {
                    b = reader.ReadByte();
                    if (b != 0)
                    {
                        writer.Write(b);
                    }
                } while (b != 0);

                return encoding.GetString(stream.ToArray());
            }
        }
    }
}

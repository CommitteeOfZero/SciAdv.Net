using SciAdvNet.Common;
using System.IO;

namespace SciAdvNet.CriwareVfs
{
    internal sealed class CpkTableHeader
    {
        public const int Size = 32;

        private CpkTableHeader(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                Signature = new string(reader.ReadChars(4));
                TableSize = reader.ReadInt32();
                EntriesOffset = reader.ReadInt32BE() + 8;
                StringsOffset = reader.ReadInt32BE() + 8;
                DataOffset = reader.ReadInt32BE() + 8;

                reader.SkipBytes(4); // table type?
                FieldCount = reader.ReadInt16BE();
                EntrySize = reader.ReadInt16BE();
                EntryCount = reader.ReadInt32BE();
            }
        }

        public string Signature { get; }
        public int TableSize { get; }
        public int EntriesOffset { get; }
        public int StringsOffset { get; }
        public int DataOffset { get; }
        public int FieldCount { get; }
        public int EntrySize { get; }
        public int EntryCount { get; }


        public static CpkTableHeader Parse(byte[] bytes)
        {
            return new CpkTableHeader(bytes);
        }
    }
}

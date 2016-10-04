using SciAdvNet.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SciAdvNet.CriwareVfs
{
    internal sealed class CpkTable<TEntry> where TEntry : CpkTableEntry
    {
        private const string UtfSignature = "@UTF";

        private readonly BinaryReader _reader;
        private readonly List<CpkTableField> _tableFields;

        private CpkTable(byte[] bytes)
        {
            _tableFields = new List<CpkTableField>();

            using (var stream = new MemoryStream(bytes))
            using (_reader = new BinaryReader(stream))
            {
                var headerBytes = _reader.ReadBytes(CpkTableHeader.Size);
                Header = CpkTableHeader.Parse(headerBytes);

                ReadTableFields();
                ReadTableEntries();
            }
        }

        public CpkTableHeader Header { get; }
        public ImmutableArray<TEntry> Entries { get; private set; }

        private long Position
        {
            get { return _reader.BaseStream.Position; }
            set { _reader.BaseStream.Position = value; }
        }


        public static CpkTable<TEntry> Parse(byte[] bytes)
        {
            string signature = new string(bytes.Take(4).Select(x => Convert.ToChar(x)).ToArray());
            if (signature != UtfSignature)
            {
                return Parse(CpkTableEncryption.Decrypt(bytes));
            }

            return new CpkTable<TEntry>(bytes);
        }

        private void ReadTableFields()
        {
            for (int i = 0; i < Header.FieldCount; i++)
            {
                byte flags = _reader.ReadByte();
                if (flags == 0)
                {
                    _reader.SkipBytes(3);
                    continue;
                }

                int nameOffset = _reader.ReadInt32BE();
                string name = ReadString(nameOffset);

                var field = new CpkTableField(flags, name);
                if (field.ContainsData)
                {
                    _tableFields.Add(field);
                }
            }
        }

        private void ReadTableEntries()
        {
            Position = Header.EntriesOffset;
            var bldEntries = ImmutableArray.CreateBuilder<TEntry>();
            for (int i = 0; i < Header.EntryCount; i++)
            {
                Position = Header.EntriesOffset + (i * Header.EntrySize);

                var bldFields = ImmutableDictionary.CreateBuilder<string, object>();
                foreach (var tableField in _tableFields)
                {
                    if (tableField.Type == CpkTableFieldType.UnsignedNumber)
                    {
                        int size = tableField.Size;
                        var bytes = _reader.ReadBytesReverse(size).Concat(Enumerable.Repeat<byte>(0, 8 - size)).ToArray();

                        bldFields[tableField.Name] = BitConverter.ToUInt64(bytes, 0);
                    }
                    else if (tableField.Type == CpkTableFieldType.String)
                    {
                        bldFields[tableField.Name] = ReadString(_reader.ReadInt32BE());
                    }
                    else
                    {
                        int offset = Header.DataOffset + _reader.ReadInt32BE();
                        _reader.ReadInt32BE();
                    }
                }
                
                var entry = (TEntry)Activator.CreateInstance(typeof(TEntry), bldFields.ToImmutable());
                bldEntries.Add(entry);
            }

            Entries = bldEntries.ToImmutable();
        }

        private string ReadString(int offset)
        {
            long oldPosition = Position;
            Position = Header.StringsOffset + offset;
            string str = _reader.ReadNullTerminatedString();
            Position = oldPosition;

            return str;
        }
    }
}

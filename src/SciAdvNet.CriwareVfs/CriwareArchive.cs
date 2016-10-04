using SciAdvNet.Common;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace SciAdvNet.CriwareVfs
{
    public sealed class CriwareArchive : IDisposable
    {
        public const string CpkSignature = "CPK ";

        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly bool _leaveOpen;

        private CpkTable<CpkFileEntry> _toc;

        private CriwareArchive(Stream stream, bool leaveOpen)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;
            _reader = new BinaryReader(_stream, Encoding.UTF8, leaveOpen);

            string signature = new string(_reader.ReadChars(4));
            if (signature != CpkSignature)
            {
                ReleaseResources();
                throw new InvalidDataException("The CPK format signature is not present in the specified stream.");
            }

            ReadHeader();
            ReadToc();
        }

        public CpkHeader Header { get; private set; }
        public ImmutableArray<CpkFileEntry> Entries => _toc.Entries;

        internal Stream ArchiveStream => _stream;

        public static CriwareArchive Load(Stream stream, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return new CriwareArchive(stream, leaveOpen);
        }

        public static CriwareArchive Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var stream = File.OpenRead(path);
            return new CriwareArchive(stream, leaveOpen: false);
        }

        public CpkFileEntry GetEntry(int id)
        {
            if (id < 0 || id >= Entries.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var entry = Entries.SingleOrDefault(x => x.Id == id);
            if (entry == null)
            {
                throw new ArgumentException(nameof(id));
            }

            return entry;
        }

        public CpkFileEntry GetEntry(string name)
        {
            var entry = Entries.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (entry == null)
            {
                throw new ArgumentException(nameof(name));
            }

            return entry;
        }

        public bool ContainsFile(string name)
        {
            return Entries.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void Dispose()
        {
            ReleaseResources();
        }

        private void ReadHeader()
        {
            _reader.SkipBytes(4);
            int cpkHeaderSize = (int)_reader.ReadInt64();
            var cpkHeaderBytes = _reader.ReadBytes(cpkHeaderSize);
            Header = CpkTable<CpkHeader>.Parse(cpkHeaderBytes).Entries.First();
        }

        private void ReadToc()
        {
            _reader.SkipBytes((int)Header.TocOffset + 4 + 4 + 8);
            var tocBytes = _reader.ReadBytes((int)Header.TocSize);
            _toc = CpkTable<CpkFileEntry>.Parse(tocBytes);

            foreach (var entry in _toc.Entries)
            {
                entry.Archive = this;
            }
        }

        private void ReleaseResources()
        {
            if (!_leaveOpen)
            {
                _reader?.Dispose();
                _stream?.Dispose();
            }
        }
    }
}
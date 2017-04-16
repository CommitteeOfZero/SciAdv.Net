using SciAdvNet.Common;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace SciAdvNet.Vfs.Criware
{
    public sealed class CriwareArchive : IArchive, IDisposable
    {
        public const string CpkSignature = "CPK ";

        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly bool _leaveOpen;

        private CpkTable<CpkFileEntry> _toc;

        private CriwareArchive(Stream stream, ArchiveMode mode, bool leaveOpen)
        {
            ArchiveMode = mode;
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
        public ImmutableArray<IFileEntry> Entries { get; private set; }
        public ArchiveMode ArchiveMode { get; }
        public bool IsCompressed => false;

        internal Stream ArchiveStream => _stream;

        public static CriwareArchive Load(Stream stream, ArchiveMode mode, bool leaveOpen = false)
        {
            if (mode == ArchiveMode.Update)
            {
                throw new NotImplementedException("Update mode is not yet supported.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException("The specified stream must me readable.", nameof(stream));
            }
            if (mode == ArchiveMode.Update && !stream.CanWrite)
            {
                throw new ArgumentException("The specified stream must be writeable.", nameof(stream));
            }

            return new CriwareArchive(stream, mode, leaveOpen);
        }

        public static CriwareArchive Load(string path, ArchiveMode mode)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var stream = File.OpenRead(path);
            return new CriwareArchive(stream, mode, leaveOpen: false);
        }

        public IFileEntry GetEntry(int id)
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

        public IFileEntry GetEntry(string name)
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

            Entries = _toc.Entries.As<IFileEntry>();
        }

        private void ReleaseResources()
        {
            if (!_leaveOpen)
            {
                _reader?.Dispose();
                _stream?.Dispose();
            }
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}

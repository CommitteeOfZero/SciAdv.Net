using System.IO;
using System.IO.Compression;

namespace SciAdvNet.Vfs.Mages
{
    public sealed partial class MpkFileEntry : IFileEntry
    {
        private Stream _compressedData;
        private MemoryStream _uncompressedData;

        internal MpkFileEntry(IArchive archive, int id, string name, long offset, long length, long compressedLength, CompressionMethod compressionMethod)
        {
            Archive = archive;
            Id = id;
            Name = name;
            DataOffset = offset;
            Length = length;
            CompressedLength = compressedLength;
            CompressionMethod = compressionMethod;
        }

        public IArchive Archive { get; }
        internal long FileHeaderOffset { get; set; }

        public int Id { get; }
        public string Name { get; }
        public long DataOffset { get; internal set; }
        public long Length { get; internal set; }
        public long CompressedLength { get; internal set; }
        public CompressionMethod CompressionMethod { get; internal set; }

        internal MemoryStream UncompressedData
        {
            get
            {
                if (_uncompressedData == null)
                {
                    _uncompressedData = new MemoryStream((int)Length);
                    using (var entryStream = OpenRead())
                    {
                        try
                        {
                            entryStream.CopyTo(_uncompressedData);
                            _uncompressedData.Position = 0;
                        }
                        catch (InvalidDataException)
                        {
                            _uncompressedData.Dispose();
                            _uncompressedData = null;
                        }
                    }
                }

                return _uncompressedData;
            }
            set
            {
                _uncompressedData = value;
            }
        }

        internal Stream CompressedData
        {
            get
            {
                if (_compressedData == null)
                {
                    if (CompressionMethod == CompressionMethod.Uncompressed)
                    {
                        _compressedData = new WrappedStream(UncompressedData);
                    }
                    else if (CompressionMethod == CompressionMethod.Zlib)
                    {
                        _compressedData = new DeflateStream(UncompressedData, CompressionMode.Compress);
                    }

                    _compressedData.Position = 0;
                }

                return _compressedData;
            }
            set
            {
                _compressedData = value;
            }
        }

        public Stream Open()
        {
            var magesArchive = Archive as MagesArchive;
            switch (magesArchive.ArchiveMode)
            {
                case ArchiveMode.Read:
                    return OpenRead();

                case ArchiveMode.Update:
                default:
                    UncompressedData.Position = 0;
                    return new WrappedStream(UncompressedData);
            }
        }

        private Stream OpenRead()
        {
            var magesArchive = Archive as MagesArchive;
            if (CompressionMethod == CompressionMethod.Uncompressed)
            {
                return new SubReadStream(magesArchive.ArchiveStream, DataOffset, Length);
            }
            else
            {
                // Skipping first 2 bytes (zlib header)
                var subStream = new SubReadStream(magesArchive.ArchiveStream, DataOffset + 2, CompressedLength - 2);
                return new DeflateStream(subStream, CompressionMode.Decompress);
            }
        }
    }
}

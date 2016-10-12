using SciAdvNet.Vfs;
using System.Collections.Immutable;
using System.IO;

namespace SciAdvNet.CriwareVfs
{
    public sealed class CpkFileEntry : CpkTableEntry, IFileEntry
    {
        public CpkFileEntry(ImmutableDictionary<string, object> fileAttributes)
            : base(fileAttributes)
        {
            
        }

        public IArchive Archive { get; internal set; }
        private CriwareArchive CriwareArchive => Archive as CriwareArchive;

        [CpkTableField("ID")]
        public int Id { get; private set; }
        [CpkTableField("DirName")]
        public string DirectoryName { get; private set; }
        [CpkTableField("FileName")]
        public string Name { get; private set; }
        [CpkTableField("FileSize")]
        public long Length { get; private set; }
        public long CompressedLength => Length;
        public long DataOffset => CriwareArchive.Header.TocOffset + RelativePosition;

        
        [CpkTableField("FileOffset")]
        private long RelativePosition { get; set; }

        public Stream Open()
        {
            return new SubReadStream(CriwareArchive.ArchiveStream, DataOffset, Length);
        }

        public override string ToString() => $"{Name} (ID = {Id}, Size = {Length})";
    }
}

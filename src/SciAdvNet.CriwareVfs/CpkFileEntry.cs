using System.Collections.Immutable;
using System.IO;

namespace SciAdvNet.CriwareVfs
{
    public sealed class CpkFileEntry : CpkTableEntry
    {
        public CpkFileEntry(ImmutableDictionary<string, object> fileAttributes)
            : base(fileAttributes)
        {
            
        }

        [CpkTableField("ID")]
        public int Id { get; private set; }
        [CpkTableField("DirName")]
        public string DirectoryName { get; private set; }
        [CpkTableField("FileName")]
        public string Name { get; private set; }
        [CpkTableField("FileSize")]
        public long Length { get; private set; }
        public long Offset => Archive.Header.TocOffset + RelativePosition;

        public CriwareArchive Archive { get; internal set; }


        [CpkTableField("FileOffset")]
        private long RelativePosition { get; set; }

        public Stream Open()
        {
            return new SubReadStream(Archive.ArchiveStream, Offset, Length);
        }

        public override string ToString() => $"{Name} (ID = {Id}, Size = {Length})";
    }
}

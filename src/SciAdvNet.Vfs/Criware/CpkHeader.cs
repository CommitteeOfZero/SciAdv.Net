using System.Collections.Immutable;

namespace SciAdvNet.Vfs.Criware
{
    public sealed class CpkHeader : CpkTableEntry
    {
        public CpkHeader(ImmutableDictionary<string, object> entryFields)
            : base(entryFields)
        {
        }

        public long ContentOffset { get; private set; }
        public long ContentSize { get; private set; }
        public long TocOffset { get; private set; }
        public long TocSize { get; private set; }
        public long TocCrc { get; private set; }
        public long EtocOffset { get; private set; }
        public long EtocSize { get; private set; }
        public long ItocOffset { get; private set; }
        public long ItocSize { get; private set; }

        [CpkTableField("Files")]
        public long FileCount { get; set; }

        public ImmutableDictionary<string, object> ExtraFields => _extraFields;
    }
}

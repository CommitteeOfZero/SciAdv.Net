using System;
using System.Collections.Immutable;

namespace SciAdvNet.Vfs
{
    public interface IArchive : IDisposable
    {
        ImmutableArray<IFileEntry> Entries { get; }
        ArchiveMode ArchiveMode { get; }

        IFileEntry GetEntry(int id);
        IFileEntry GetEntry(string name);

        void SaveChanges();
    }
}

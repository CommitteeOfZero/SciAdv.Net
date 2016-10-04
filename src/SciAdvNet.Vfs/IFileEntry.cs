using System.IO;

namespace SciAdvNet.Vfs
{
    public interface IFileEntry
    {
        int Id { get; }
        string Name { get; }
        long DataOffset { get;}
        long Length { get; }
        long CompressedLength { get; }

        IArchive Archive { get; }

        Stream Open();
    }
}

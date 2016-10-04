using System.IO;
using System.IO.Compression;
using Xunit;

namespace SciAdvNet.SC3.Tests
{
    public sealed class SteinsGateHDTests
    {
        [Fact]
        public void DecodingStringsWorks()
        {
            var archive = ZipFile.OpenRead("Data/SteinsGateHD.zip");
            foreach (var entry in archive.Entries)
            {
                using (var fileStream = OpenScript(entry))
                {
                    SC3Module module = SC3Module.Load(fileStream);
                    foreach (var stringHandle in module.StringTable)
                    {
                        stringHandle.Resolve();
                    }
                }
            }
        }

        private Stream OpenScript(ZipArchiveEntry entry)
        {
            using (var entryStream = entry.Open())
            {
                var seekable = new MemoryStream((int)entryStream.Length);
                entryStream.CopyTo(seekable);
                seekable.Position = 0;
                return seekable;
            }
        }
    }
}

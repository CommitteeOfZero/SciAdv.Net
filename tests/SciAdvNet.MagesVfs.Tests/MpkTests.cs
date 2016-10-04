using SciAdvNet.Vfs;
using System.IO;
using System.Security.Cryptography;
using Xunit;

namespace SciAdvNet.MagesVfs.Tests
{
    public sealed class MpkTests
    {
        [Fact]
        public void ReadCompressedV1()
        {
            var archive = MagesArchive.Load("Data/CompressedV1.mpk", ArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                Assert.Contains(".scr", entry.Name);

                using (var stream = entry.Open())
                using (var reader = new BinaryReader(stream))
                {
                    string signature = new string(reader.ReadChars(4));
                    Assert.Equal("SC3\0", signature);
                }
            }
        }

        [Fact]
        public void ReadUncompressedV2()
        {
            var archive = MagesArchive.Load("Data/UncompressedV2.mpk", ArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                Assert.Contains(".scx", entry.Name);

                using (var stream = entry.Open())
                using (var reader = new BinaryReader(stream))
                {
                    string signature = new string(reader.ReadChars(4));
                    Assert.Equal("SC3\0", signature);
                }
            }
        }

        [Fact]
        public void RepackUncompressedV2()
        {
            var original = File.OpenRead("Data/UncompressedV2.mpk");
            var repacked = new MemoryStream((int)original.Length);
            original.CopyTo(repacked);

            using (var a = MagesArchive.Load(repacked, ArchiveMode.Update, leaveOpen: true))
            {
                a.SaveChanges();
            }

            using (var sha1 = SHA1.Create())
            {
                original.Position = 0;
                repacked.Position = 0;
                var correctHash = sha1.ComputeHash(original);
                var repackedHash = sha1.ComputeHash(repacked);

                original.Dispose();
                repacked.Dispose();

                Assert.Equal(correctHash, repackedHash);
            }
        }
    }
}

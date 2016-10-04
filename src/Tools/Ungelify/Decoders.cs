using SciAdvNet.Common;
using SciAdvNet.CriwareVfs;
using SciAdvNet.MagesVfs;
using SciAdvNet.Vfs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ungelify
{
    public abstract class Decoder
    {
        private static readonly List<Decoder> SupportedDecoders = new List<Decoder>()
        {
            //new CpkDecoder(),
            new MpkDecoder()
        };

        public static Decoder Recognize(Stream stream)
        {
            var result = SupportedDecoders.SingleOrDefault(x => x.IsSupportedArchive(stream));
            if (result == null)
            {
                throw new InvalidDataException("Unrecognized format.");
            }

            return result;
        }

        public abstract bool IsSupportedArchive(Stream stream);
        public abstract IArchive LoadArchive(Stream stream);
    }

    //public sealed class CpkDecoder : Decoder
    //{
    //    public override bool IsSupportedArchive(Stream stream)
    //    {
    //        using (var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
    //        {
    //            return new string(reader.PeekChars(4)) == CriwareArchive.CpkSignature;
    //        }
    //    }

    //    public override IArchive LoadArchive(Stream stream)
    //    {
    //        return new CpkArchive(stream);
    //    }
    //}

    public sealed class MpkDecoder : Decoder
    {
        public override bool IsSupportedArchive(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
            {
                return new string(reader.PeekChars(4)) == MagesArchive.MpkSignature;
            }
        }

        public override IArchive LoadArchive(Stream stream)
        {
            return MagesArchive.Load(stream, ArchiveMode.Update);
        }
    }
}

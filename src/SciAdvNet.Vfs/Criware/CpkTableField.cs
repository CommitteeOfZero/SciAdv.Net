using System.IO;

namespace SciAdvNet.Vfs.Criware
{
    internal sealed class CpkTableField
    {
        private const byte StorageMask = 0xf0;
        private const int TypeMask = 0x0f;

        private const byte StringTypeCode = 0xA;
        private const byte ArrayTypeCode = 0xB;

        private readonly int _flags;

        internal CpkTableField(byte flags, string name)
        {
            _flags = flags;
            Name = name;
            ContainsData = (flags & StorageMask) == 0x50;

            DetermineType();
        }

        public string Name { get; }
        public CpkTableFieldType Type { get; private set; }
        public int Size { get; private set; }
        public bool ContainsData { get; }


        private void DetermineType()
        {
            int cpkTypeCode = _flags & TypeMask;
            int numericTypeSize;
            if ((numericTypeSize = GetNumericTypeSize(cpkTypeCode)) != -1)
            {
                Type = CpkTableFieldType.UnsignedNumber;
                Size = numericTypeSize;
            }
            else
            {
                switch (cpkTypeCode)
                {
                    case StringTypeCode:
                        Type = CpkTableFieldType.String;
                        break;

                    case ArrayTypeCode:
                        Type = CpkTableFieldType.ByteArray;
                        break;

                    default:
                        throw new InvalidDataException("Encountered a CPK table field of an unknown type.");
                }
            }
        }

        private static int GetNumericTypeSize(int typeCode)
        {
            switch (typeCode)
            {
                case 0: case 1: return 1;
                case 2: case 3: return 2;
                case 4: case 5: return 4;
                case 6: case 7: return 8;
                case 8: return 4;

                default:
                    return -1;
            }
        }
    }

    internal enum CpkTableFieldType
    {
        UnsignedNumber = 0,
        String = 1,
        ByteArray = 2
    };
}

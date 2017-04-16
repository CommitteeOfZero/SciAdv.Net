using System;
using System.Collections.Generic;

namespace SciAdvNet.Vfs.Criware
{
    internal sealed class CpkTableField
    {
        private static readonly Dictionary<int, int> NumericTypeSizes = new Dictionary<int, int>()
        {
            [0] = 1,
            [1] = 1,
            [2] = 2,
            [3] = 2,
            [4] = 4,
            [5] = 4,
            [6] = 8,
            [7] = 8,
            [8] = 4
        };

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
            if (NumericTypeSizes.ContainsKey(cpkTypeCode))
            {
                Type = CpkTableFieldType.UnsignedNumber;
                Size = NumericTypeSizes[cpkTypeCode];
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
                        throw new NotSupportedException("Encountered an unknown data field type.");
                }
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

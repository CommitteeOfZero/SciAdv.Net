using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SciAdvNet.SC3.Utils
{
    internal sealed class ByteArrayComparer : EqualityComparer<ImmutableArray<byte>>
    {
        private static readonly ByteArrayComparer Comparer = new ByteArrayComparer();
        public static ByteArrayComparer Instance => Comparer;

        public override bool Equals(ImmutableArray<byte> x, ImmutableArray<byte> y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            return x.SequenceEqual(y);
        }

        public override int GetHashCode(ImmutableArray<byte> obj)
        {
            unchecked
            {
                int hash = 17;
                foreach (byte b in obj)
                {
                    hash = hash * 31 + b;
                }

                return hash;
            }
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Utilities
{
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals([AllowNull] byte[] a, [AllowNull] byte[] b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            if (a.Length != b.Length) 
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) 
                    return false;

            return true;
        }

        public int GetHashCode([DisallowNull] byte[] a)
        {
            uint b = 0;

            for (int i = 0; i < a.Length; i++)
                b = ((b << 23) | (b >> 9)) ^ a[i];

            return unchecked((int)b);
        }
    }
}

using System;
using System.Collections.Generic;

namespace Search2.Util
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        public EqualityComparer(Func<T, T, bool> cmp)
        {
            Cmp = cmp;
        }
        public bool Equals(T x, T y)
        {
            return Cmp(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public Func<T, T, bool> Cmp { get; set; }
    }
}
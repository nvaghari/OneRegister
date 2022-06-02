using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Comparer
{
    internal class PermissionModelComparer<T1, T2> : IEqualityComparer<KeyValuePair<T1, T2>>
        where T1 : IComparable
    {
        public bool Equals(KeyValuePair<T1, T2> x, KeyValuePair<T1, T2> y)
        {
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode([DisallowNull] KeyValuePair<T1, T2> keyvalue)
        {
            return keyvalue.Key.GetHashCode();
        }
    }
}

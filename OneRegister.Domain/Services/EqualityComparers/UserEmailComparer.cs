using OneRegister.Data.Identication;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OneRegister.Domain.Services.EqualityComparers
{
    public class UserEmailComparer : IEqualityComparer<OUser>
    {
        public bool Equals(OUser x, OUser y)
        {
            return string.CompareOrdinal(x.Email, y.Email) == 0;
        }

        public int GetHashCode([DisallowNull] OUser user)
        {
            return user.GetHashCode();
        }
    }
}

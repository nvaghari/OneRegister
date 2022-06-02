using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.EqualityComparers
{
    public class CodeListComparer : IEqualityComparer<CodeList>
    {
        public bool Equals(CodeList x, CodeList y)
        {
            return x.Name == y.Name && x.Value == y.Value;
        }

        public int GetHashCode([DisallowNull] CodeList codelist)
        {
            return codelist.Name.GetHashCode() ^ codelist.Value.GetHashCode();
        }
    }
}

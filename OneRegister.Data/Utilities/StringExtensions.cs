using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Utilities
{
    static class StringExtensions
    {

        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text.Length <= maxLength ? text : text.Substring(0, maxLength);
        }
    }
}

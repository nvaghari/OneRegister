using System;
using System.Globalization;
using System.Linq;

namespace OneRegister.Domain.Extentions
{
    public static class StringExtensions
    {
        public static DateTime ToDate(this string dateStr, string format = "yyyyMMdd")
        {
            DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthday);
            return birthday;
        }
        public static string Sanitize(this string text)
        {
            return text.Trim();
        }
        public static string ArrayToString(this string[] textArray)
        {
            if (textArray.Length == 0)
            {
                return string.Empty;
            }

            return textArray.Aggregate((a, b) => a + "," + b);
        }
        public static string SanitizeForJavaScript(this string text)
        {
            var prohibitedList =new  char[] {' ','&','!','@','#','$','%','^','*','(',')','-','+','='};
            var textArray = text.ToCharArray().ToList();
            textArray.RemoveAll(c => prohibitedList.Contains(c));
            return new string(textArray.ToArray());
        }
    }
}

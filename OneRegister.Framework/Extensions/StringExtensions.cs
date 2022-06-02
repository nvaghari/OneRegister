using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Web.Services.Extensions
{
    public static class StringExtensions
    {
        public static string ToMyGridDateTimeString(this DateTime d)
        {
            try
            {
                return d.ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                return "invalid date";
            }
        }

        public static string ControllerName(this string str)
        {
            return str.Replace("Controller", "");
        }

        public static string ToErrorMessage(this ICollection<string> errors)
        {
            return errors.Aggregate((a, b) => a + Environment.NewLine + b);
        }

        //public static string ToJsObject(this MerchantRegisterState states)
        //{
        //    return "{a:\"test\"}";
        //}
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "is not in correct format";
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }
            else
            {
                return Regex.Match(value.ToString(), @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").Success;
            }
        }
    }
}

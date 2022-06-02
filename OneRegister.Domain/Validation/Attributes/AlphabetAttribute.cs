using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AlphabetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "Only alphabet are allowed";
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }
            else
            {
                return Regex.Match(value.ToString(), @"^[a-zA-Z ]*$").Success;
            }
        }
    }
}

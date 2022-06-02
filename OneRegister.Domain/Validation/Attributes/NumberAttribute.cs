using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "Only digits are allowed";
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }
            else
            {
                return Regex.Match(value.ToString(), @"^\d+$").Success;
            }
        }
    }
}

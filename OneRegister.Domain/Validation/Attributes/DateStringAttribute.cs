using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OneRegister.Domain.Validation.Attributes
{
    public class DateStringAttribute : ValidationAttribute
    {
        public string Format { get; set; } = "yyyyMMdd";
        public override bool IsValid(object value)
        {
            ErrorMessage = $"Date should be in this format: {Format}";
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }
            return DateTime.TryParseExact(value.ToString(), Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}

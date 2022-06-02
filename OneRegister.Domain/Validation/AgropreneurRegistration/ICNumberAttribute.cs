using OneRegister.Domain.Model.AgropreneurRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.AgropreneurRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ICNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (AGPImportModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(value?.ToString())
                || string.IsNullOrEmpty(model.IdentityType)
                || model.IdentityType != "NRIC")
            {
                return ValidationResult.Success;
            }

            if (value.ToString().Length != 12)
            {
                return new ValidationResult("IC number should be 12 digits");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}

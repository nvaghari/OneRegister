using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.StudentRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SchoolCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var service = (StudentService)validationContext.GetService(typeof(StudentService));
            if (!service.IsSchoolExist(value.ToString()))
            {
                return new ValidationResult("School doesn't exist or you don't have access");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

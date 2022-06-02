using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.StudentRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StudentIdentityNumberUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (StudentService)validationContext.GetService(typeof(StudentService));
            var model = (StudentImportModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(value.ToString())
                || string.IsNullOrEmpty(model.School)
                || string.IsNullOrEmpty(model.IdentityType))
            {
                return ValidationResult.Success;
            }

            if (service.IsIdentityNumberExist(model.School, model.IdentityType, value.ToString()))
            {
                return new ValidationResult("Identity Number does exist");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

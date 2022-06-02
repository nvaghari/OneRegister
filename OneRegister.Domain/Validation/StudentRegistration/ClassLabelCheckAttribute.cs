using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.StudentRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ClassLabelCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (StudentService)validationContext.GetService(typeof(StudentService));
            var model = (StudentImportModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(value?.ToString())
                || string.IsNullOrEmpty(model.Class)
                || string.IsNullOrEmpty(model.Year))
            {
                return ValidationResult.Success;
            }
            if (!service.IsLabelValid(value.ToString(), model.Class, Convert.ToInt32(model.Year)))
            {
                return new ValidationResult("ClassLabel is not Match");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

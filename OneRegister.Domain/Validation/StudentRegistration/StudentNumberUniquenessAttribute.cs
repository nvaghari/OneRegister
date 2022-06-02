using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.StudentRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StudentNumberUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (StudentService)validationContext.GetService(typeof(StudentService));
            var model = (StudentImportModel)validationContext.ObjectInstance;

            if (string.IsNullOrEmpty(model.School) || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if (service.IsStudentNumberExist(model.School, value.ToString()))
            {
                return new ValidationResult("StudentNumber does exist");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.StudentRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StudentNameUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (StudentService)validationContext.GetService(typeof(StudentService));
            var model = (StudentImportModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(value.ToString()) || string.IsNullOrEmpty(model.School))
            {
                return ValidationResult.Success;
            }

            if (service.IsStudentNameExist(model.School, value.ToString()))
            {
                return new ValidationResult("Student Name does exist");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

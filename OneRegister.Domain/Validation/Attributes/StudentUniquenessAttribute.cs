using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StudentUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var studentBusiness = (StudentService)validationContext.GetService(typeof(StudentService));
            var model = (StudentRegisterModel)validationContext.ObjectInstance;
            if (model.SchoolId == null || string.IsNullOrEmpty(model.Name))
            {
                return ValidationResult.Success;
            }
            if (studentBusiness.IsDuplicate(model))
            {
                return new ValidationResult("This student name has been registered before");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

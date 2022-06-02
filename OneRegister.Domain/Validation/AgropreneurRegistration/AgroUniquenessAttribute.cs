using OneRegister.Domain.Model.AgropreneurRegistration;
using OneRegister.Domain.Services.AgropreneurRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.AgropreneurRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class AgroUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var agroBusiness = (AgropreneurService)validationContext.GetService(typeof(AgropreneurService));
            var model = (AGPRegisterModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(model.PlotNo))
            {
                return ValidationResult.Success;
            }
            if (agroBusiness.IsDuplicate(model))
            {
                return new ValidationResult("This Agropreneur has been registered before");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

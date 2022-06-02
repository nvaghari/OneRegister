using OneRegister.Domain.Model.AgropreneurRegistration;
using OneRegister.Domain.Services.AgropreneurRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.AgropreneurRegistration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PlotNoUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (AgropreneurService)validationContext.GetService(typeof(AgropreneurService));
            var model = (AGPImportModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return ValidationResult.Success;
            }

            if (service.IsPlotNumberExist(value.ToString()))
            {
                return new ValidationResult("PlotNo does exist");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

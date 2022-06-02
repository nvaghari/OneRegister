using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Domain.Services.MerchantRegistration;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MerchantUniquenessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var merchantBusiness = (MerchantService)validationContext.GetService(typeof(MerchantService));
            var model = (MerchantRegisterModel_Info)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(model.BusinessNo) || string.IsNullOrEmpty(model.RegisteredBusiness))
            {
                return ValidationResult.Success;
            }
            if (merchantBusiness.IsDuplicate(model))
            {
                return new ValidationResult("This Merchant has been registered before (RegisteredName or BusinessNo)");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

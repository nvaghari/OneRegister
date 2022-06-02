using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantConfigurationRegisterModel
    {
        [Display(Name = "Terms & Conditions")]
        [CustomRequired]
        public string TermsAndConditions { get; set; }
    }
}

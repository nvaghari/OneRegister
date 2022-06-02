using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Prepaid
    {
        [Display(Name = "VAS products to be excluded", Description = "if any")]
        public string PrepaidDetail { get; set; }
    }
}

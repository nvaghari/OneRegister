using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Payment
    {
        [Display(Name = "Payment Methods to be excluded", Description = "if any")]
        public string PaymentDetail { get; set; }
    }
}

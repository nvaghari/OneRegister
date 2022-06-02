using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Bank : MerchantPartialDataModel
    {
        [Display(Name = "Bank Name")]
        [CustomRequired]
        [Alphabet]
        public string BankName { get; set; }

        [Display(Name = "Bank Account Name")]
        [CustomRequired]
        public string AccountName { get; set; }

        [Display(Name = "Bank Account No")]
        [CustomRequired]
        [Number]
        public string AccountNo { get; set; }

        [Display(Name = "Branch Address")]
        public string BankAddress { get; set; }

        [Display(Name = "Do you agree to display promotional materials on your premises and website?")]
        [CustomRequired]
        public BankPromotionalPremisesType? BankPromoAgree { get; set; }
    }
}

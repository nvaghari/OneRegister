using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterRejectModel : MerchantPartialDataModel
    {
        [Display(Name = "Please tell the merchant why you are rejecting")]
        [CustomRequired]
        public string Remark { get; set; }
        [Display(Name = "Is this a permanent rejection?", Description = "checking this box means you want to reject the merchant completely")]
        public bool IsPermanent { get; set; }
    }
}

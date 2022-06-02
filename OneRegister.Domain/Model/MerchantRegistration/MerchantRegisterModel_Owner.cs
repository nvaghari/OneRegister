using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Owner : MerchantPartialDataModel
    {

        [Display(Name = "Name")]
        [CustomRequired]
        public string OwnerName { get; set; }

        [Display(Name = "Designation")]
        public string OwnerDesignation { get; set; }

        [Display(Name = "IC No/Passport")]
        [CustomRequired]
        [MaxLength(20)]
        public string OwnerIdentityNo { get; set; }

        [Display(Name = "MobileNo")]
        [CustomRequired]
        [Number]
        public string OwnerMobile { get; set; }
    }
}

using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.MerchantRegistration
{
    public class MerchantOwnerViewModel
    {
        public virtual Guid? OwnerId { get; set; }
        public virtual Guid? Mid { get; set; }

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
        public Dictionary<string, string> DesignationList { get; set; }
    }
}

using OneRegister.Domain.Validation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class SalesPersonFormRegistrationModel
    {
        [CustomRequired]
        [Display(Name = "Business No")]
        public string BusinessNo { get; set; }
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [CustomRequired]
        [Display(Name = "Merchant User Account", Description = "if you can not find the merchant user, first you need to create their account")]
        public string MerchantAccountId { get; set; }
        public Guid MerchantAccountGuid => Guid.TryParse(MerchantAccountId, out Guid id) ? id : Guid.Empty; 
        public MerchantRegisterModel_Services Services { get; set; } = new();
        public string FormNumber { get; set; }

        public string ServicesStr => JsonSerializer.Serialize(Services);
    }
}

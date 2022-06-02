using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Channel : MerchantPartialDataModel
    {
        [CustomRequired]
        [Display(Name = "Website and/or Mobile Apps (IP Address)")]
        public string ChannelAddress { get; set; }

        [CustomRequired]
        [Display(Name = "URL")]
        public string ChannelUrl { get; set; }


        [CustomRequired]
        [Email]
        [Display(Name = "Email")]
        public string ChannelEmail { get; set; }
    }
}

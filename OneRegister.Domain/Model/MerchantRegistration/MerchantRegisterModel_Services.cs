using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Services : MerchantPartialDataModel
    {
        [JsonIgnore]
        public override Guid? Id { get => base.Id; set => base.Id = value; }
        [JsonIgnore]
        public override Guid? Mid { get => base.Mid; set => base.Mid = value; }


        [Display(Name = "VAS on M1 Device")]
        public bool OnePay_VasOnM1Device { get; set; }

        [Display(Name = "VAS on M1 WebPortal")]
        public bool OnePay_VasOnM1WebPortal { get; set; }

        [Display(Name = "VAS on M1 SmartPhone App")]
        public bool OnePay_VasOnM1SmartphoneApp { get; set; }

        [Display(Name = "VAS on API")]
        public bool OnePay_VasOnApi { get; set; }

        [Display(Name = "VAS on Retailer Device")]
        public bool OnePay_VasOnRetailerDevice { get; set; }

        [Display(Name = "MyDebit")]
        public bool OnePay_MyDebit { get; set; }

        [Display(Name = "eM-ONEi")]
        public bool OnePay_Emonei { get; set; }

        [Display(Name = "Credit Card on Device Paydee")]
        public bool OnePay_CreditCardOnDevicePaydee { get; set; }

        [Display(Name = "Credit Card on Device UMS")]
        public bool OnePay_CreditCardOnDeviceUms { get; set; }

        [Display(Name = "eWallets")]
        public bool OnePay_Ewallets { get; set; }

        [Display(Name = "Axiata Aspirasi")]
        public bool OnePay_AxiataAspirasi { get; set; }

        [Display(Name = "DuitNow QR")]
        public bool OnePay_DuitNowQR { get; set; }

        [Display(Name = "Credit Card (UMS)")]
        public bool M1Pay_CreditCardUms { get; set; }

        [Display(Name = "FPX")]
        public bool M1Pay_Fpx { get; set; }

        [Display(Name = "eM-ONEi")]
        public bool M1Pay_Emonei { get; set; }

        [Display(Name = "eWallets")]
        public bool M1Pay_Ewallets { get; set; }

        [Display(Name = "Alipay")]
        public bool M1Pay_Alipay { get; set; }
    }
}

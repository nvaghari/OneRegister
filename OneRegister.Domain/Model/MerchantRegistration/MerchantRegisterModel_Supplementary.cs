using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Supplementary : MerchantPartialDataModel
    {
        [Display(Name = "Company")]
        public string Company { get; set; }
        [Display(Name = "Comp RegNo")]
        public string CompanyRegisterNo { get; set; }
        [Display(Name = "BPCode AR")]
        public string BPCodeAR { get; set; }
        [Display(Name = "BPCOde AP")]
        public string BPCodeAP { get; set; }
        [Display(Name = "RefNo")]
        public string RefNo { get; set; }
        [Required]
        [Display(Name = "MCC")]
        public string MCC { get; set; }
        [Required]
        [Display(Name = "Cheque No")]
        public string ChequeNo { get; set; }
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }
        [Display(Name = "Risk Level")]
        public string RiskLevel { get; set; }
        [Display(Name = "Risk Comments")]
        public string RiskComments { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "Monthly Rental (if any)")]
        public string MonthlyRental { get; set; }
    }
}

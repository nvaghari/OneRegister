using OneRegister.Domain.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_File : MerchantPartialDataModel
    {
        [Display(Name = "SSM/Lesen Perniagaan/Certificate of Society")]
        public string CompanyRegistrationSearch { get; set; }
        public string CompanyRegistrationSearchUrl { get; set; }

        [Display(Name = "Identification documents (Passport/NRIC) of all board of directors,shareholders and authorized signatory")]
        [CustomRequired]
        public string IdentificationDocuments { get; set; }
        public string IdentificationDocumentsUrl { get; set; }

        [Display(Name = "Latest month of bank statement with beneficiary name, address and account number")]
        public string BankStatement { get; set; }
        public string BankStatementUrl { get; set; }

        [Display(Name = "One Copy exterior photo of applicant's business premises front view with signboard and One copy interior photos of applicant's business premises inside view with products")]
        public string ApplicantPhoto { get; set; }
        public string ApplicantPhotoUrl { get; set; }

        [Display(Name = "For Society(Pertubuhan/Koperasi) Latest Organization Chart")]
        public string CtosOfBoard { get; set; }
        public string CtosOfBoardUrl { get; set; }

        [Display(Name = "Any other relevant information upon company's reasonable request from time to time")]
        public string OtherDocument { get; set; }
        public string OtherDocumentUrl { get; set; }

        [Display(Name = "Commercial Rate (for Sales person/account manager use only)")]
        [CustomRequired]
        public string CommercialRate { get; set; }
        public string CommercialRateUrl { get; set; }
    }
}

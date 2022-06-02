using Microsoft.AspNetCore.Http;
using OneRegister.Data.Contract;
using OneRegister.Domain.Model.General;
using OneRegister.Domain.Validation.AgropreneurRegistration;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.AgropreneurRegistration
{
    public class AGPRegisterModel : PersonRegisterModel
    {
        [Display(Name = "ID / Plot number")]
        [CustomRequired]
        [AgroUniqueness]
        public string PlotNo { get; set; }

        [Display(Name = "Company Name")]
        [CustomRequired]
        public string Company { get; set; }

        [Display(Name = "SSM No")]
        [CustomRequired]
        public string CompanyNo { get; set; }

        [Display(Name = "Designation")]
        [CustomRequired]
        public string Designation { get; set; }
        public Dictionary<string, string> DesignationList { get; set; }
        [Display(Name = "Industry")]
        [CustomRequired]
        public string Industry { get; set; }
        public Dictionary<string, string> IndustryList { get; set; }

        [Display(Name = "Company Bank Account")]
        [CustomRequired]
        public string BankName { get; set; }
        public Dictionary<string, string> BankNameList { get; set; }

        [Display(Name = "Account No")]
        [CustomRequired]
        public string AccountNo { get; set; }

        [Display(Name = "Nature Of Business")]
        [CustomRequired]
        public string NatureOfBusiness { get; set; }
        public Dictionary<string, string> NatureOfBusinessList { get; set; }

        [Display(Name = "Purpose Of Transaction")]
        [CustomRequired]
        public string PurposeOfTransaction { get; set; }
        public Dictionary<string, string> PuproseOfTransactionList { get; set; }

        [Display(Name = "Identity Photo", Description = "please provide both front and back photo of ID card")]
        [Image]
        public IFormFile IdPhoto { get; set; }
        public long? IdPhotoDms { get; set; }
        public string IdPhotoUrl { get; set; }
        [Image]
        public IFormFile Photo { get; set; }
        public long? PhotoDms { get; set; }
        public string PhotoUrl { get; set; }

        public IFormFile PhotoC { get; set; }


        public IFormFile PhotoT { get; set; }
        public StateOfEntity State { get; internal set; }

        [Display(Name = "Entry Date")]
        public DateTime? EntryDate { get; set; }
        [Display(Name = "Visa Expiry Date")]
        public DateTime? VisaExpiry { get; set; }
        [Display(Name = "PLKS Expiry Date", Description = "PLKS Expiry Date")]
        public DateTime? PlksExpiry { get; set; }
        [Display(Name = "Term Of Service", Description = "in Months")]
        public int? TermOfService { get; set; }
    }
}

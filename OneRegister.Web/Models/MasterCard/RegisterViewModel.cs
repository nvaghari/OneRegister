using Microsoft.AspNetCore.Http;
using OneRegister.Domain.Model.Enum.MasterCard;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.MasterCard
{
    public class RegisterViewModel
    {
        public AddressViewModel HomeAddress { get; set; } = new();
        public AddressViewModel PostAddress { get; set; } = new();
        public string HomeAddressJson { get; set; }
        public string PostAddressJson { get; set; }
        [Display(Name ="Home Address is same as Postal Address")]
        public bool IsAddressSame { get; set; }
        [Display(Name = "IC Source")]
        public string ICSource { get; set; }
        public Dictionary<string, string> ICSourceList { get; set; }
        public string Channel { get; set; }
        public Dictionary<string, string> ChannelList { get; set; }
        public string[] ListPackages { get; set; }
        public Dictionary<string, string> ListPackagesList { get; set; }
        public string CustAuthMode { get; set; }
        [Display(Name ="Organization ID")]
        public Guid? OrgID { get; set; }
        public string Title { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "First Name")]
        [CustomRequired]
        public string FirstName { get; set; }
        public string MiddleName1 { get; set; }
        public string MiddleName2 { get; set; }
        [Display(Name = "Last Name")]
        [CustomRequired]
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public Dictionary<string, string> NationalityList { get; set; }
        public string NID { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportExpiry { get; set; }
        public DateTime? PassportIssuingDate { get; set; }
        public string PassportIssuingPlace { get; set; }
        public string PassportIssuingAuthority { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string BankBIC { get; set; }
        public Dictionary<string, string> BankBICList { get; set; }
        public string BankAcctNo { get; set; }
        public string BankAcctName { get; set; }
        [Display(Name = "Occupation Type")]
        public string OccType { get; set; }
        public Dictionary<string, string> OccTypeList { get; set; }
        [Display(Name = "Occupation Industry")]
        public string OccIndustry { get; set; }
        public Dictionary<string, string> OccIndustryList { get; set; }
        [Display(Name = "Occupation Position")]
        public string OccPosition { get; set; }
        [Display(Name = "Occupation Company")]
        public string OccCompany { get; set; }
        public string MotherName { get; set; }
        public string FaceDmsId { get; set; }
        [Display(Name = "Face Photo")]
        public IFormFile FaceDmsFile { get; set; }
        public string DocPage1DmsId { get; set; }
        [Display(Name = "Front MYKAD Photo")]
        public IFormFile DocPage1DmsFile { get; set; }
        public string DocPage2DmsId { get; set; }
        [Display(Name = "Back MYKAD Photo")]
        public IFormFile DocPage2DmsFile { get; set; }
        public string XUser { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using OneRegister.Domain.Model.Enum.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.Model
{
    public class MasterCardRegisterModel
    {
        public MasterCardAddressModel HomeAddress { get; set; } = new();
        public MasterCardAddressModel PostAddress { get; set; } = new();
        public string HomeAddressJson { get; set; }
        public string PostAddressJson { get; set; }
        public bool IsAddressSame { get; set; }
        public string ICSource { get; set; }
        public string Channel { get; set; }
        public string[] ListPackages { get; set; }
        public string CustAuthMode { get; set; }
        public Guid? OrgID { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName1 { get; set; }
        public string MiddleName2 { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
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
        public string BankAcctNo { get; set; }
        public string BankAcctName { get; set; }
        public string OccType { get; set; }
        public string OccIndustry { get; set; }
        public string OccPosition { get; set; }
        public string OccCompany { get; set; }
        public string MotherName { get; set; }
        public string FaceDmsId { get; set; }
        public IFormFile FaceDmsFile { get; set; }
        public string DocPage1DmsId { get; set; }
        public IFormFile DocPage1DmsFile { get; set; }
        public string DocPage2DmsId { get; set; }
        public IFormFile DocPage2DmsFile { get; set; }
        public string XUser { get; set; }
    }
}

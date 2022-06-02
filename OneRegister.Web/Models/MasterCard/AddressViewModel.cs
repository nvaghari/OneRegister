using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.MasterCard
{
    public class AddressViewModel
    {
        [Display(Name = "Address Line 1")]
        public string Addr1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string Addr2 { get; set; }
        [Display(Name = "Address Line 3")]
        public string Addr3 { get; set; }
        public string Suburb { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }
        [Display(Name = "StateCode")]
        public string StateCode { get; set; }
        public Dictionary<string, string> StateCodeList { get; set; }
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }
        public Dictionary<string, string> CountryCodeList { get; set; }
        public string GpsLatitude { get; set; }
        public string GpsLongitude { get; set; }
        public string AddrType { get; set; }
        public Dictionary<string, string> AddrTypeList { get; set; }
        public int AddrId { get; set; }
    }
}

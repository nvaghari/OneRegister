using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Outlet : MerchantPartialDataModel
    {
        [Display(Name = "Outlet/Shop Name")]
        [CustomRequired]
        public string OName { get; set; }

        [Display(Name = "Outlet TelNo")]
        [CustomRequired]
        [Number]
        public string OTelNo { get; set; }

        [Display(Name = "Outlet Address")]
        [CustomRequired]
        public string OAddress { get; set; }

        [Display(Name = "Outlet FaxNo")]
        [Number]
        public string OFaxNo { get; set; }

        [Display(Name = "Town")]
        [CustomRequired]
        [Alphabet]
        public string OTown { get; set; }

        [Display(Name = "State")]
        [CustomRequired]
        public string OState { get; set; }

        [Display(Name = "Postcode")]
        [CustomRequired]
        [Number]
        public string OPostCode { get; set; }

        [Display(Name = "Latitude")]
        [Decimal]
        public decimal? OLat { get; set; }

        [Display(Name = "Longitude")]
        [Decimal]
        public decimal? OLng { get; set; }

        [Display(Name = "Outlet Shop Type")]
        [CustomRequired]
        public OutletType? OType { get; set; }

        [Display(Name = "Outlet Operating Days and Hours", Description = "eg: Mon-Fri, 9am-6pm")]
        public string OOperatingDaysHours { get; set; }

        [Display(Name = "Contact Person")]
        [CustomRequired]
        public string OContactPerson { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string OPEmail { get; set; }

        [Display(Name = "Mobile")]
        [CustomRequired]
        [Number]
        public string OPMobileNo { get; set; }

        [Display(Name = "Tel")]
        [CustomRequired]
        [Number]
        public string OPTelNo { get; set; }

        [Display(Name = "Remarks")]
        public string ORemarks { get; set; }
    }
}

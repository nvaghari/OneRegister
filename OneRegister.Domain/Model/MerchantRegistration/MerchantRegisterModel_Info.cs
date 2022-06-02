using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Validation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel_Info : MerchantPartialDataModel
    {
        [Display(Name = "Registered Business")]
        [CustomRequired]
        [MerchantUniqueness]
        public string RegisteredBusiness { get; set; }

        [Display(Name = "Business Registration No")]
        [CustomRequired]
        public string BusinessNo { get; set; }

        [Display(Name = "SST/GST ID", Description = "registrant only")]
        public string SstId { get; set; }

        [Display(Name = "Registered Address")]
        [CustomRequired]
        public string Address { get; set; }

        [Display(Name = "Country Of Registration")]
        [CustomRequired]
        public string Country { get; set; }

        [Display(Name = "Town")]
        [CustomRequired]
        [Alphabet]
        public string Town { get; set; }

        [CustomRequired]
        [Display(Name = "State")]
        public string AreaState { get; set; }

        [CustomRequired]
        [Number]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Type Of Business")]
        [CustomRequired]
        public BusinessType? BusinessType { get; set; }

        [Display(Name = "Principal Business")]
        public string Principal { get; set; }

        [Display(Name = "Type Of Product & Services")]
        public string ProductType { get; set; }

        [Display(Name = "Business Operating Days & Hours", Description = "eg: Mon-Fri, 9am-6pm")]
        public string OperatingDaysHours { get; set; }

        [Display(Name = "Average Ticket Size (RM)")]
        [CustomRequired]
        [Decimal]
        public decimal? TickectSize { get; set; }

        [Display(Name = "Expected Monthly Turnover (RM)")]
        [CustomRequired]
        [Decimal]
        public decimal? MonthlyTurnover { get; set; }

        [CustomRequired]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Display(Name = "Mobile No")]
        [CustomRequired]
        [Number]
        public string MobileNo { get; set; }

        [Display(Name = "Email")]
        [CustomRequired]
        [Email]
        public string Email { get; set; }

        [Display(Name = "Designation")]
        [CustomRequired]
        public string Designation { get; set; }

        [Display(Name = "Tel No")]
        [Number]
        public string TelNo { get; set; }

        [Display(Name = "Fax No")]
        [Number]
        public string FaxNo { get; set; }

        [Display(Name = "Sales Person/Account Manager")]
        [CustomRequired]
        public Guid? SalesPersonId { get; set; }

        [Display(Name = "Estimated delivery time of goods and / or services to the customers", Description = "eg. Immediately/number of days")]
        public string DeliveryTime { get; set; }
    }
}

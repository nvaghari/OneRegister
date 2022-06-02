using OneRegister.Data.Entities.MerchantRegistration;
using System;

namespace OneRegister.Data.Model.MerchantRegistration
{
    public class POCO_MerchantInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BusinessNo { get; set; }
        public string SstId { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string CountryState { get; set; }
        public string PostCode { get; set; }
        public BusinessType? BusinessType { get; set; }
        public string Principal { get; set; }
        public string ProductType { get; set; }
        public string OperatingDaysHours { get; set; }
        public decimal? TickectSize { get; set; }
        public decimal? MonthlyTurnover { get; set; }
        public string ContactName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string FormNumber { get; set; }
        public MerchantRegisterState MerchantState { get; set; }
        public int FormNumberBase { get; set; }
        public string PassedStates { get; set; }
        public MerchantStatus MerchantStatus { get; set; }
        public Guid? SalesPersonId { get; set; }
    }
}

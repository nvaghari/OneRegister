using System;

namespace OneRegister.Data.Model.MerchantRegistration
{
    public class MerchantListModel
    {
        public Guid Id { get; set; }
        public string FormNo { get; set; }
        public string RegisteredBusiness { get; set; }
        public string BusinessNo { get; set; }
        public string BusinessType { get; set; }
        public string Salesperson { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
    }
}

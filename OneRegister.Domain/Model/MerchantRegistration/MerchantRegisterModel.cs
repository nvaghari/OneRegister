using OneRegister.Data.Entities.MerchantRegistration;
using System;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantRegisterModel
    {
        public MerchantRegisterState MerchantState { get; set; }
        public MerchantStatus MerchantStatus { get; set; }
        public string MerchantStatusUser { get; set; }
        public Guid? Id { get; set; }
        public string FormNo { get; set; }
        public MerchantRegisterModel_Services Services { get; set; }
        public MerchantRegisterModel_Info Info { get; set; }
        public MerchantRegisterModel_Bank Bank { get; set; }
        public MerchantRegisterModel_Channel Channel { get; set; }
        public MerchantRegisterModel_File Files { get; set; }
        public MerchantRegisterRejectModel Reject { get; set; }
    }
}

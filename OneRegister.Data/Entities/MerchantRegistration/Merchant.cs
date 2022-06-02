using OneRegister.Data.SuperEntities;
using System.Collections.Generic;

namespace OneRegister.Data.Entities.MerchantRegistration
{
    public class Merchant : Organization
    {
        public MerchantInfo MerchantInfo { get; set; }
        public ICollection<MerchantOutlet> MerchantOutlets { get; set; }
        public ICollection<MerchantOwner> MerchantOwners { get; set; }
        public MerchantCommission MerchantCommission { get; set; }
    }
}

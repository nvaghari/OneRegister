using OneRegister.Data.SuperEntities;
using System;

namespace OneRegister.Data.Entities.MerchantRegistration
{
    public class MerchantOwner : Member
    {
        private Guid merchantId;


        public Merchant Merchant { get; set; }
        public Guid MerchantId
        {
            get
            {
                return merchantId;
            }

            set
            {
                OrganizationId = value;
                merchantId = value;
            }
        }
    }
}

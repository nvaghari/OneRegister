using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.MerchantRegistration
{
    public enum OutletType
    {
        Other, Kiosk, Branch, HeadQuarters
    }
    public class MerchantOutlet : Site
    {
        private Guid merchantId;


        [StringLength(DataLength.Name)]
        public string ContactPerson { get; set; }
        [StringLength(DataLength.PHONE)]
        public string ContactTel { get; set; }
        [StringLength(DataLength.PHONE)]
        public string ContactMobile { get; set; }
        [StringLength(DataLength.EMAIL)]
        public string ContactEmail { get; set; }
        public OutletType OutletType { get; set; }

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

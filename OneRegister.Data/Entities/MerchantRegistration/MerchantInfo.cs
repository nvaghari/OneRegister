using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.MerchantRegistration
{
    public enum BusinessType
    {
        Others, SoleProprietor, Partnership, SdnBhd, Bhd
    }
    public enum BankPromotionalPremisesType
    {
        No = 0, Yes = 1
    }
    //should be same as JS file
    public enum MerchantRegisterState
    {
        New = 0,
        Info = 1,
        Services = 2,
        Owners = 3,
        Outlets = 4,
        BankAccount = 5,
        Files = 6,
        Supplementary = 7,
        Complete = 8,
        Channel = 9,
        Terms = 10,
        Commission = 11
    }
    public enum MerchantStatus
    {
        Incomplete,
        Complete,
        Op1,
        Op2,
        Risk,
        Rejected,
        Inadequate,
        Accept

    }
    [Table(nameof(MerchantInfo), Schema = SchemaNames.Merchant)]
    public class MerchantInfo : BaseEntity,IGenericEntity
    {
        [StringLength(DataLength.ShortJson)]
        public string Services { get; set; }

        [StringLength(DataLength.SHORTNAME)]
        public string PassedStates { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string BusinessNo { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string SstId { get; set; }
        [StringLength(DataLength.ADDRESS)]
        public string Address { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Country { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Town { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string AreaState { get; set; }
        [StringLength(DataLength.PostCode)]
        public string PostCode { get; set; }
        public BusinessType? BusinessType { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Principal { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string ProductType { get; set; }
        [StringLength(DataLength.Description)]
        public string OperatingDaysHours { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TickectSize { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MonthlyTurnover { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Designation { get; set; }
        [StringLength(DataLength.PHONE)]
        public string TelNo { get; set; }
        [StringLength(DataLength.PHONE)]
        public string FaxNo { get; set; }
        public MerchantRegisterState MerchantState { get; set; }

        public MerchantStatus MerchantStatus { get; set; }
        public string MerchantStatusUser
        {
            get
            {
                return MerchantStatus switch
                {
                    MerchantStatus.Incomplete => MerchantStatus.Incomplete.ToString(),
                    MerchantStatus.Complete => MerchantStatus.Complete.ToString(),
                    MerchantStatus.Op1 => "Processing",
                    MerchantStatus.Op2 => "Processing",
                    MerchantStatus.Risk => "Processing",
                    MerchantStatus.Rejected => MerchantStatus.Rejected.ToString(),
                    MerchantStatus.Inadequate => MerchantStatus.Inadequate.ToString(),
                    MerchantStatus.Accept => MerchantStatus.Accept.ToString(),
                    _ => "Unknown",
                };
            }
        }

        [StringLength(DataLength.IDENTITY)]
        public string FormNumber { get; set; }
        public int FormNumberBase { get; set; }

        //Merchant Account
        [StringLength(DataLength.Name)]
        public string ContactName { get; set; }
        [StringLength(DataLength.PHONE)]
        public string MobileNo { get; set; }
        [StringLength(DataLength.EMAIL)]
        public string Email { get; set; }
        public Guid? Account { get; set; }

        //Bank
        [StringLength(DataLength.LONGNAME)]
        public string BankName { get; set; }
        [StringLength(DataLength.Name)]
        public string BankAccountName { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string BankAccountNo { get; set; }
        [StringLength(DataLength.ADDRESS)]
        public string BankAddress { get; set; }
        public bool? BankPromoAgreeable { get; set; }

        //channel
        [StringLength(DataLength.ADDRESS)]
        public string ChannelAddress { get; set; }
        [StringLength(DataLength.URL)]
        public string ChannelUrl { get; set; }

        [StringLength(DataLength.EMAIL)]
        public string ChannelEmail { get; set; }

        public Guid? SalesPersonId { get; set; }

        [StringLength(DataLength.Remark)]
        public string RejectRemark { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string DeliveryTime { get; set; }

        //navigate properties
        public Guid MerchantId { get; set; }
        public Merchant Merchant { get; set; }

    }
}

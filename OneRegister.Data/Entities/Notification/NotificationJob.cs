using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.Notification
{
    public enum ActionType
    {
        NewMerchantRegistered,
        MerchantFilesUploaded,
        CommissionProvided,
        MerchantCompleted,
        OP1Rejected,
        OP1Accepted,
        OP2Rejected,
        OP2Accepted,
        RiskRejected,
        RiskAccepted,
        Rejected
    }

    [Table(nameof(NotificationJob), Schema = SchemaNames.Notification)]
    public class NotificationJob : BaseEntity,IGenericEntity
    {
        public Guid? RefId { get; set; }
        public ActionType ActionType { get; set; }
        [StringLength(DataLength.Description)]
        public string Result { get; set; }
    }
}

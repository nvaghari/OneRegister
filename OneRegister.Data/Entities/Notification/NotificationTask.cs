using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.Notification
{
    public enum NotificationType
    {
        Email,
        SMS,
        MerchantOnboardAPI
    }
    [Table(nameof(NotificationTask), Schema = SchemaNames.Notification)]
    public class NotificationTask : BaseEntity,IGenericEntity
    {
        public NotificationType NotificationType { get; set; }
        [StringLength(DataLength.EMAIL)]
        public string To { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string Subject { get; set; }
        [StringLength(DataLength.Remark)]
        public string Message { get; set; }
        [StringLength(DataLength.Description)]
        public string Result { get; set; }
        public NotificationJob NotificationJob { get; set; }
        public Guid? NotificationJobId { get; set; }
    }
}

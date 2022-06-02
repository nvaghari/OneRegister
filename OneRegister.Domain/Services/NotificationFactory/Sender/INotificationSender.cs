using OneRegister.Data.Entities.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.NotificationFactory.Sender
{
    public interface INotificationSender
    {
        bool IsEligible(NotificationType notificationType);
        void Send(NotificationTask notification);
    }
}

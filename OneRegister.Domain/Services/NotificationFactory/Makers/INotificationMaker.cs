using OneRegister.Data.Entities.Notification;

namespace OneRegister.Domain.Services.NotificationFactory.Makers
{
    public interface INotificationMaker
    {
        bool IsEligible(ActionType actionType);
        void Make(NotificationJob notificationJob);
    }
}

using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.Notification;
using OneRegister.Domain.Services.Email;
using System;

namespace OneRegister.Domain.Services.NotificationFactory.Sender
{
    public class EmailSender : INotificationSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly NotificationService _notificationService;
        private readonly EmailService _emailService;

        public EmailSender(
            ILogger<EmailSender> logger,
            NotificationService notificationService,
            EmailService emailService
            )
        {
            _logger = logger;
            this._notificationService = notificationService;
            this._emailService = emailService;
        }
        public bool IsEligible(NotificationType notificationType)
        {
            return notificationType == NotificationType.Email;
        }

        public void Send(NotificationTask notification)
        {
            try
            {
                _emailService.Send(notification);
                _notificationService.TaskDone(notification.Id);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"sending email was failed for task id: {notification.Id}");
                _notificationService.TaskFail(notification.Id, "Exception Error");
            }
        }
    }
}

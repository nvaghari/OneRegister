using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.Notification;
using OneRegister.Domain.Services.NotificationFactory;
using OneRegister.Domain.Services.NotificationFactory.Sender;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Services.ScheduleTask.Jobs
{
    public class TaskGrabberJob
    {
        private readonly ILogger<TaskGrabberJob> _logger;
        private readonly NotificationService _notificationService;
        private readonly IEnumerable<INotificationSender> _notificationSenders;

        public TaskGrabberJob(
            ILogger<TaskGrabberJob> logger,
            NotificationService notificationService,
            IEnumerable<INotificationSender> notificationSenders
            )
        {
            _logger = logger;
            _notificationService = notificationService;
            _notificationSenders = notificationSenders;
        }
        public void Run()
        {
            if (!_notificationService.IsContextConnected())
            {
                _logger.LogWarning("Can not connect to Database");
                return;
            }

            var tasks =  _notificationService.GetInProgressTasks();
            if (tasks.Any())
            {
                _logger.LogInformation($"{tasks.Count} in-progress task(s) was found");
                BuildNotifications(tasks);
            }
        }

        private void BuildNotifications(List<NotificationTask> tasks)
        {
            foreach (var task in tasks)
            {
                foreach (var sender in _notificationSenders)
                {
                    if (sender.IsEligible(task.NotificationType))
                    {
                        sender.Send(task);
                    }
                }
            }
        }
    }
}

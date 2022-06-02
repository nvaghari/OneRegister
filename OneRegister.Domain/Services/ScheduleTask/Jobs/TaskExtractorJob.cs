using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.Notification;
using OneRegister.Domain.Services.NotificationFactory;
using OneRegister.Domain.Services.NotificationFactory.Makers;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Services.ScheduleTask.Jobs
{
    public class TaskExtractorJob
    {
        private readonly NotificationJobService _notificationJobService;
        private readonly IEnumerable<INotificationMaker> _notificationMakers;
        private readonly ILogger<NotificationJobService> _logger;

        public TaskExtractorJob(
            NotificationJobService notificationJobService, 
            IEnumerable<INotificationMaker> notificationMakers,
            ILogger<NotificationJobService> logger)
        {
            _notificationJobService = notificationJobService;
            _notificationMakers = notificationMakers;
            _logger = logger;
        }
        public void Run()
        {
            if (!_notificationJobService.IsContextConnected())
            {
                _logger.LogWarning("Can not connect to Database");
                return;
            }

            var notifJobs = _notificationJobService.GetInProgressJobs();
            if (notifJobs.Any())
            {
                _logger.LogInformation($"{notifJobs.Count()} in-progress job(s) was found");
                BuildNotifications(notifJobs);
            }
        }
        private void BuildNotifications(List<NotificationJob> notifJobs)
        {
            foreach (var job in notifJobs)
            {
                foreach (var notifMaker in _notificationMakers)
                {
                    if (notifMaker.IsEligible(job.ActionType))
                    {
                        notifMaker.Make(job);
                    }
                }
            }
        }
    }
}

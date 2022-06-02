using Microsoft.Extensions.Logging;
using OneRegister.Domain.Services.ScheduleTask.Jobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.ScheduleTask
{
    public class NotificationDispenserJob : IJob
    {
        private readonly ILogger<NotificationDispenserJob> _logger;
        private readonly TaskExtractorJob _taskExtractorJob;
        private readonly TaskGrabberJob _taskGrabberJob;

        public NotificationDispenserJob(
            ILogger<NotificationDispenserJob> logger,
            TaskExtractorJob taskExtractorJob,
            TaskGrabberJob taskGrabberJob
            )
        {
            _logger = logger;
            _taskExtractorJob = taskExtractorJob;
            _taskGrabberJob = taskGrabberJob;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("[Scheduler] " + "Notification Job Started");
            try
            {
                _taskExtractorJob.Run();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            try
            {
                _taskGrabberJob.Run();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}

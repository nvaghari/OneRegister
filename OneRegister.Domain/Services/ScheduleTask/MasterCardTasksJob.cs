using Microsoft.Extensions.Logging;
using OneRegister.Domain.Services.MasterCard;
using OneRegister.Domain.Services.MasterCard.JobFactory;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.ScheduleTask
{
    public class MasterCardTasksJob : IJob
    {
        private readonly ILogger<MasterCardTasksJob> _logger;
        private readonly MCTasksJobService _mCTasksJobService;

        public MasterCardTasksJob(ILogger<MasterCardTasksJob> logger,MCTasksJobService mCTasksJobService)
        {
            _logger = logger;
            _mCTasksJobService = mCTasksJobService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("[Scheduler] "+"MasterCard Tasks Job Started");
            try
            {
                _mCTasksJobService.Run();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Scheduler] " + "MasterCard Tasks Job Error");
            }
            return Task.CompletedTask;
        }
    }
}

using Microsoft.Extensions.Logging;
using OneRegister.Domain.Services.MasterCard.InquiryFactory;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.ScheduleTask
{
    public class MasterCardInquiriesJob : IJob
    {
        private readonly ILogger<MasterCardInquiriesJob> _logger;
        private readonly MCInquiryService _mCInquiryService;

        public MasterCardInquiriesJob(ILogger<MasterCardInquiriesJob> logger, MCInquiryService mCInquiryService)
        {
            _logger = logger;
            _mCInquiryService = mCInquiryService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("[Scheduler] " + "MasterCard Inquiries Job Started");
            try
            {
                _mCInquiryService.Run();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[Scheduler] " + "MasterCard Inquiries Job Error");
            }
            return Task.CompletedTask;
        }
    }
}

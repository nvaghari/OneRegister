using Microsoft.Extensions.Logging;
using OneRegister.Data.Repository.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public class MCInquiryService
    {
        private readonly IEnumerable<IInquirer> _inquirers;
        private readonly MasterCardInquiryRepository _masterCardInquiryRepository;
        private readonly ILogger<MCInquiryService> _logger;

        public MCInquiryService(IEnumerable<IInquirer> inquirers, 
            MasterCardInquiryRepository masterCardInquiryRepository,
            ILogger<MCInquiryService> logger)
        {
            _inquirers = inquirers;
            _masterCardInquiryRepository = masterCardInquiryRepository;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogDebug("Start grabbing tasks...");
            var tasks = _masterCardInquiryRepository.GetInProgressInquiryTasks();
            if (tasks.Any())
            {
                _logger.LogInformation(tasks.Count + "MasterCard Inquiry task(s) was grabbed");
            }
            foreach (var task in tasks)
            {
                foreach (var inquirer in _inquirers)
                {
                    if (inquirer.IsEligible(task.InquiryType))
                    {
                        try
                        {
                            inquirer.Inquiry(task);
                        }
                        catch (Exception ex)
                        {

                            _logger.LogError(ex, "Error on Inquiry task: " + task.Id);
                        }
                    }
                }
            }
        }
    }
}

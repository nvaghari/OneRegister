using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OneRegister.Domain.Services.MasterCard.JobFactory
{
    public class GetCddActionIvListJob : IMasterCardJob
    {
        public GetCddActionIvListJob(
              AMLService aMLService,
              MasterCardInquiryRepository inquiryRepository,
              ILogger<GetCddActionIvListJob> logger)
        {
            _aMLService = aMLService;
            _inquiryRepository = inquiryRepository;
            _logger = logger;
        }
        private readonly AMLService _aMLService;
        private readonly MasterCardInquiryRepository _inquiryRepository;
        private readonly ILogger<GetCddActionIvListJob> _logger;
        public AMLService AMLService => _aMLService;

        public void Execute()
        {
            _logger.LogDebug("[IV] "+"CDDActionIV_ListRequests...");
            var customesList = _aMLService.GetCDDActionIV_ListRequests();
            if (!customesList.Any())
            {
                _logger.LogDebug("[IV] " + customesList.Count() + " IVList Record(s) was fetched");
                return;
            }
            _logger.LogInformation("[IV] " + customesList.Count() + " IVList Record(s) was fetched");

            IEnumerable<InquiryTask> tasks = GetTasks(customesList);

            _inquiryRepository.AddInquiries(tasks);
        }

        private static IEnumerable<InquiryTask> GetTasks(IEnumerable<CDDActionID_ListRequestsResult> customers)
        {
            return customers.Select(n => new InquiryTask
            {
                InquiryType = InquiryType.IV,
                InquiryName = InquiryType.IV.ToString(),
                Source = nameof(CDDActionID_ListRequestsResult),
                RefId = n.CDDActionID.ToString(),
                RefId2 = n.ProcessorTxnID,
                Name = n.FullName,
                JsonValue = JsonSerializer.Serialize(n)
            });
        }
    }
}

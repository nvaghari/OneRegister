using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.JobFactory
{
    public class GetSanctionScreeningNamesJob : IMasterCardJob
    {
        private readonly AMLService _aMLService;
        private readonly MasterCardInquiryRepository _inquiryRepository;
        private readonly ILogger<GetSanctionScreeningNamesJob> _logger;
        public GetSanctionScreeningNamesJob(
            AMLService aMLService,
            MasterCardInquiryRepository inquiryRepository,
            ILogger<GetSanctionScreeningNamesJob> logger
            )
        {
            _aMLService = aMLService;
            _inquiryRepository = inquiryRepository;
            _logger = logger;
        }
        public void Execute()
        {
            _logger.LogDebug("[SS] " + "GetSSTxn_ListRequestResult...");
            var names = _aMLService.GetSSTxn_ListRequestResult();
            if (!names.Any())
            {
                _logger.LogDebug("[SS] " + names.Count() + " SSList Record(s) was fetched");
                return;
            }
            _logger.LogInformation("[SS] " + names.Count() + " SSList Record(s) was fetched");

            IEnumerable<InquiryTask> tasks = GetTasks(names);
            _inquiryRepository.AddInquiries(tasks);
        }

        private static IEnumerable<InquiryTask> GetTasks(List<SSTxn_ListRequestsResult> names)
        {
            return names.Select(n => new InquiryTask { 
                InquiryType = InquiryType.SS,
                InquiryName = InquiryType.SS.ToString(),
                Source = nameof(SSTxn_ListRequestsResult),
                RefId = n.CDDActionSS.ToString(),
                Name = n.FullName,
                JsonValue = JsonSerializer.Serialize(n)
            });
        }
    }
}

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
    public class GetCddActionDvListJob : IMasterCardJob
    {
        private readonly ILogger<GetCddActionDvListJob> _logger;
        private readonly AMLService _aMLService;
        private readonly MasterCardInquiryRepository _inquiryRepository;

        public GetCddActionDvListJob(
            ILogger<GetCddActionDvListJob> logger,
            AMLService aMLService,
            MasterCardInquiryRepository inquiryRepository)
        {
            _logger = logger;
            _aMLService = aMLService;
            _inquiryRepository = inquiryRepository;
        }
        public void Execute()
        {
            _logger.LogDebug("[DV] " + "GetCDDActionDV_ListResults...");
            var dvList = _aMLService.GetCDDActionDV_ListResults(GemStatus.I);
            if (!dvList.Any())
            {
                _logger.LogDebug("[DV] " + dvList.Count() + " DVList Record(s) was fetched");
                return;
            }
            _logger.LogInformation("[DV] " + dvList.Count() + " DVList Record(s) was fetched");

            IEnumerable<InquiryTask> tasks = GetTasks(dvList);
            _inquiryRepository.AddInquiries(tasks);
        }

        private static IEnumerable<InquiryTask> GetTasks(IEnumerable<CDDActionDV_ListResult> dvList)
        {
            return dvList.Select(l => new InquiryTask
            {
                InquiryType = InquiryType.DV,
                InquiryName = InquiryType.DV.ToString(),
                Source = nameof(CDDActionDV_ListResult),
                RefId = l.CDDActionDV.ToString(),
                Name = l.EntityFullName,
                JsonValue = JsonSerializer.Serialize(l)
            });
        }
    }
}

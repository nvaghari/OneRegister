using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Services.KYCApi;
using OneRegister.Domain.Services.KYCApi.ErrorHandling;
using OneRegister.Domain.Services.KYCApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public class SanctionScreeningInquirer : IInquirer
    {
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly ILogger<SanctionScreeningInquirer> _logger;
        private readonly KYCService _kYCService;

        public SanctionScreeningInquirer(
            MasterCardTasksRepository masterCardTasksRepository,
            ILogger<SanctionScreeningInquirer> logger,
            KYCService kYCService
            )
        {
            _masterCardTasksRepository = masterCardTasksRepository;
            _logger = logger;
            _kYCService = kYCService;
        }
        public void Inquiry(InquiryTask inquiryTask)
        {
            var taskId = inquiryTask.Id.ToString();
            try
            {
                _logger.LogInformation($"[SS][Start][TaskId:{taskId}] {inquiryTask.JsonValue}");
                var spResult = JsonSerializer.Deserialize<SSTxn_ListRequestsResult>(inquiryTask.JsonValue);
                var model = new SSRequestModel()
                {
                    FirstName = spResult.FirstName,
                    LastName = spResult.LastName,
                    BirthDate = spResult.DOB?.ToString("yyyy-MM-dd")
                };
                _logger.LogDebug($"[SS][TaskId:{taskId}] " + "calling SS API");
                var sanctionResult = _kYCService.GetSanctionscreening(model);
                _logger.LogDebug($"[SS][TaskId:{taskId}] SS API response: " + sanctionResult);

                var resultModel = JsonSerializer.Deserialize<SSResultModel>(sanctionResult, KYCService.SerializeOption);
                if (string.IsNullOrEmpty(resultModel.CheckId))
                {
                    throw new KycException(System.Net.HttpStatusCode.OK, "watchlist-report", sanctionResult);
                }
                _masterCardTasksRepository.MarkAsFetched(inquiryTask.Id, resultModel.CheckId);

            }
            catch (KycException ex)
            {
                _logger.LogError($"[SS][KYCErr][TaskId:{taskId}] " + ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source, ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, nameof(DVInquirer), ex.Message);
            }
        }

        public bool IsEligible(InquiryType inquiryType)
        {
            return inquiryType == InquiryType.SS;
        }
    }
}

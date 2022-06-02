using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Exceptions;
using OneRegister.Domain.Services.KYCApi;
using OneRegister.Domain.Services.KYCApi.ErrorHandling;
using OneRegister.Domain.Services.RPPApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public class IVInquirer : IInquirer
    {
        private readonly ILogger<IVInquirer> _logger;
        private readonly KYCService _kycService;
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly AMLService _aMLService;

        public IVInquirer(ILogger<IVInquirer> logger,
           KYCService kycService,
           MasterCardTasksRepository masterCardTasksRepository,
           AMLService aMLService)
        {
            _logger = logger;
            _kycService = kycService;
            _masterCardTasksRepository = masterCardTasksRepository;
            _aMLService = aMLService;
        }
        public void Inquiry(InquiryTask inquiryTask)
        {
            var spResult = new CDDActionID_ListRequestsResult();
            var taskId = inquiryTask.Id.ToString();
            try
            {
                _logger.LogInformation($"[IV][Start][TaskId:{taskId}] {inquiryTask.JsonValue}");
                spResult = JsonSerializer.Deserialize<CDDActionID_ListRequestsResult>(inquiryTask.JsonValue);
                var ivReportResult = _kycService.GetIVReport(inquiryTask.RefId2);
                _logger.LogDebug($"[IV][TaskId:{taskId}] IV API response: "+"{@ivReportResult}",ivReportResult);
                //if (!string.IsNullOrEmpty(ivReportResult.VerificationResult) && ivReportResult.VerificationResult == "UNDEFINED")
                //{
                //    //calling check API and mark the job as waiting
                //}
                _logger.LogDebug($"[IV][TaskId:{taskId}] "+ "CDDActionID_EKyc_SetVeriStatusL1");
                _aMLService.SetIVVeriStatusL1(ivReportResult,Convert.ToInt32(inquiryTask.RefId),inquiryTask.RefId2);

                _masterCardTasksRepository.MarkAsSuccess(inquiryTask.Id);
            }
            catch (KycException ex)
            {
                _logger.LogDebug($"[IV][KYCErr][TaskId:{taskId}] " + ex.Message);
                _aMLService.SetIVVeriStatusL1(spResult.CDDActionID, ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source, ex.Code, ex.Message);
            }
            catch (GemsException ex)
            {
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source, ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, nameof(DVInquirer), ex.Message);
            }
        }

        public bool IsEligible(InquiryType inquiryType)
        {
            return inquiryType == InquiryType.IV;
        }
    }
}

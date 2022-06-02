using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Exceptions;
using OneRegister.Domain.Services.KYCApi;
using OneRegister.Domain.Services.KYCApi.ErrorHandling;
using OneRegister.Domain.Services.KYCApi.Model;
using System;
using System.Text.Json;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public class DVInquirer : IInquirer
    {
        private readonly KYCService _kYCService;
        private readonly ILogger<DVInquirer> _logger;
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly AMLService _aMLService;

        public DVInquirer(
            KYCService kYCService, 
            ILogger<DVInquirer> logger,
            MasterCardTasksRepository masterCardTasksRepository,
            AMLService aMLService)
        {
            _kYCService = kYCService;
            _logger = logger;
            _masterCardTasksRepository = masterCardTasksRepository;
            _aMLService = aMLService;
        }
        public void Inquiry(InquiryTask inquiryTask)
        {
            var spResult = new CDDActionDV_ListResult();
            var taskId = inquiryTask.Id.ToString();
            try
            {
                _logger.LogInformation($"[DV][Start][TaskId:{taskId}] {inquiryTask.JsonValue}");
                spResult = JsonSerializer.Deserialize<CDDActionDV_ListResult>(inquiryTask.JsonValue);
                var model = new DVRequestModel
                {
                    DocumentType = spResult.DocType,
                    DocumentUri = spResult.DocImg1,
                    BackSideDocumentUri = spResult.DocImg2
                };
                _logger.LogDebug($"[DV][TaskId:{taskId}] " + "calling DV API");
                var kycResultStr = _kYCService.GetDocumentVerification(model);
                var userKey = _kYCService.GetUserKeyFromDVResult(kycResultStr);
                _logger.LogDebug($"[DV][TaskId:{taskId}] DV API response: " + kycResultStr);

                _logger.LogDebug($"[DV][TaskId:{taskId}] " + "CDDActionDV_SetVeriStatusL1");
                _aMLService.SetDVVeriStatusL1(spResult.CDDActionDV, "I", userKey, kycResultStr);


                _masterCardTasksRepository.MarkAsFetched(inquiryTask.Id, userKey);

            }
            catch (KycException ex)
            {
                _logger.LogError($"[DV][KYCErr][TaskId:{taskId}] " + ex.Message);
                _aMLService.SetVeriStatusL1(spResult.CDDActionDV, ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source,ex.Code, ex.Message);
            }
            catch (GemsException ex)
            {
                _logger.LogError($"[DV][GEMErr][TaskId:{taskId}] " + ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source,ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, nameof(DVInquirer), ex.Message);
            }
        }

        public bool IsEligible(InquiryType inquiryType)
        {
            return inquiryType == InquiryType.DV;
        }
    }
}

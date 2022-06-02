using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Exceptions;
using OneRegister.Domain.Services.RPPApi;
using OneRegister.Domain.Services.RPPApi.ErrorHandling;
using System;
using System.Text.Json;

namespace OneRegister.Domain.Services.MasterCard.InquiryFactory
{
    public class BankAccountInquirer : IInquirer
    {
        private readonly ILogger<BankAccountInquirer> _logger;
        private readonly RPPService _rPPService;
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly AMLService _aMLService;

        public BankAccountInquirer(ILogger<BankAccountInquirer> logger,
            RPPService rPPService,
            MasterCardTasksRepository masterCardTasksRepository,
            AMLService aMLService)
        {
            _logger = logger;
            _rPPService = rPPService;
            _masterCardTasksRepository = masterCardTasksRepository;
            _aMLService = aMLService;
        }
        public bool IsEligible(InquiryType inquiryType)
        {
            return inquiryType == InquiryType.RPP;
        }
        public void Inquiry(InquiryTask inquiryTask)
        {
            var spResult = new CDDActionIV_ListBankAcctInfoResult();
            var taskId = inquiryTask.Id.ToString();
            try
            {
                _logger.LogInformation($"[RPP][Start][TaskId:{taskId}] {inquiryTask.JsonValue}");
                spResult = JsonSerializer.Deserialize<CDDActionIV_ListBankAcctInfoResult>(inquiryTask.JsonValue);
                Model.CheckBankAccountModel bankAccountModel = new()
                {
                    AcctNo = spResult.BankAcctNo,
                    BankCode = spResult.BankBIC,
                    RefId = spResult.CDDActionIV.ToString()
                };
                _logger.LogDebug($"[RPP][TaskId:{taskId}] "+"calling RPP API");
                var rppResponse = _rPPService.CheckBankAccount(bankAccountModel);
                _logger.LogDebug($"[RPP][TaskId:{taskId}] RPP API response: " + rppResponse);
                if (rppResponse.IsSuccessful)
                {
                    _logger.LogDebug($"[RPP][TaskId:{taskId}] " + "CDDActionIV_SetBankAcctStatus");
                    _aMLService.SetBankAccountStatus(spResult.CDDActionIV, spResult.BankBIC, spResult.BankAcctNo, rppResponse.RetMsg, inquiryTask.Id.ToString(), null);

                }
                else
                {
                    _logger.LogDebug($"[RPP][TaskId:{taskId}] " + "CDDActionIV_SetBankAcctStatus");
                    _aMLService.SetBankAccountStatus(spResult.CDDActionIV, spResult.BankBIC, spResult.BankAcctNo, null, inquiryTask.Id.ToString(), rppResponse.RetMsg);
                }

                _masterCardTasksRepository.MarkAsSuccess(inquiryTask.Id);

            }
            catch (RPPException ex)
            {
                _logger.LogError($"[RPP][APIErr][TaskId:{taskId}] " + ex.Message);
                _aMLService.SetBankAccountStatus(spResult.CDDActionIV, spResult.BankBIC, spResult.BankAcctNo, null, inquiryTask.Id.ToString(), "OneRegister "+ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source, ex.Code, ex.Message);
            }
            catch (GemsException ex)
            {
                _logger.LogError($"[RPP][GEMErr][TaskId:{taskId}] " + ex.Message);
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, ex.Source, ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(inquiryTask.Id, "BankAccountInquirer", ex.Message);
            }

        }

    }
}

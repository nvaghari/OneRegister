using Microsoft.Extensions.Logging;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Exceptions;
using OneRegister.Domain.Services.KYCApi.Model;
using OneRegister.Domain.Services.MasterCard;
using System;
using System.Text.Json;

namespace OneRegister.Domain.Services.Webhooks
{
    public class WebookService
    {
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly AMLService _aMLService;
        private readonly ILogger<WebookService> _logger;
        private JsonSerializerOptions _serializeOption => new (){PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping};

        public WebookService(
            MasterCardTasksRepository masterCardTasksRepository,
            AMLService aMLService,
            ILogger<WebookService> logger)
        {
            _masterCardTasksRepository = masterCardTasksRepository;
            _aMLService = aMLService;
            _logger = logger;
        }
        public void ProcessDVResult(object model)
        {
            if (model == null)
            {
                throw new ApplicationException("The response from eKYC API for DVResult is empty");
            }
            var modelStr = JsonSerializer.Serialize(model,_serializeOption);
            _logger.LogDebug("[WHook] processing this model: " + modelStr);
            var responseModel = JsonSerializer.Deserialize<DVWebhookResponseModel>(modelStr, _serializeOption);
            var task = _masterCardTasksRepository.GetByRefId2(responseModel.UserKey);
            if (task == null)
            {
                _logger.LogError("[DV][WHook] no task for this userkey " + responseModel.UserKey);
                throw new ApplicationException("There is no task for this user key " + responseModel.UserKey);
            }

            try
            {
                string dvStatus = (responseModel.ReportDetails?.Result) switch
                {
                    "consider" => "U",
                    "clear" => "P",
                    "rejected" => "U",
                    _ => "U",
                };
                _logger.LogDebug("start calling CDDActionDV_SetVeriStatusL1 SP...");
                _aMLService.SetDVVeriStatusL1(Convert.ToInt32(task.RefId), dvStatus, responseModel.UserKey, modelStr);

                _masterCardTasksRepository.MarkAsSuccess(task.Id);
            }
            catch (GemsException ex)
            {
                _logger.LogError($"[DV][WHook][GEMErr][TaskId:{task.Id}] " + ex.Message);
                _masterCardTasksRepository.MarkAsFailure(task.Id, "CDDActionDV_SetVeriStatusL1", $"ErrorCode: {ex.ErrorNumber}");
                throw;
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(task.Id, "CDDActionDV_SetVeriStatusL1",ex.Message);
                throw;
            }
        }

        public void ProcessSSResult(object model)
        {
            if (model == null)
            {
                throw new ApplicationException("The response from eKYC API for SSResult is empty");
            }
            var modelStr = JsonSerializer.Serialize(model);
            _logger.LogDebug("[WHook] processing this model: " + modelStr);
            var responseModel = JsonSerializer.Deserialize<SSWebhookResponseModel>(modelStr, _serializeOption);
            var task = _masterCardTasksRepository.GetByRefId2(responseModel.CheckId);
            if (task == null)
            {
                _logger.LogError("[SS][WHook] no task for this checkId " + responseModel.CheckId);
                throw new ApplicationException("There is no task for this user key " + responseModel.CheckId);
            }
            try
            {
                _logger.LogDebug("start calling SSTxn_SetResultV2 SP...");
                _aMLService.SetSSResultV2(Convert.ToInt32(task.RefId), modelStr);

                _masterCardTasksRepository.MarkAsSuccess(task.Id);
            }
            catch (GemsException ex)
            {
                _logger.LogError($"[SS][WHook][GEMErr][TaskId:{task.Id}] " + ex.Message);
                _masterCardTasksRepository.MarkAsFailure(task.Id, "SSTxn_SetResultV2", $"ErrorCode: {ex.ErrorNumber}");
                throw;
            }
            catch (Exception ex)
            {
                _masterCardTasksRepository.MarkAsFailure(task.Id, "SSTxn_SetResultV2", ex.Message);
                throw;
            }
        }
    }
}

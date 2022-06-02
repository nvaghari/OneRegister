using Microsoft.AspNetCore.Mvc;
using OneRegister.Domain.Services.KYCApi.Model;
using System;
using OneRegister.Web.Services.Audit;
using OneRegister.Domain.Services.Webhooks;
using System.IO;

namespace OneRegister.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(WebhookAuditAttribute))]
    public class ListenToController : ControllerBase
    {
        private readonly WebhookLogService _webhookLogService;
        private readonly WebookService _webHookService;

        public ListenToController(
            WebhookLogService webhookLogService,
            WebookService webHookService)
        {
            _webhookLogService = webhookLogService;
            _webHookService = webHookService;
        }
        [HttpPost]
        public IActionResult KycDVResult(object model)
        {
            try
            {
                _webHookService.ProcessDVResult(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _webhookLogService.Logger.Error(ex.Message);
                return StatusCode(500, "Internal Error At " + DateTime.Now.ToLongTimeString());
            }
        }

        [HttpPost]
        public IActionResult KycSSResult(object model)
        {
            try
            {
                _webHookService.ProcessSSResult(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _webhookLogService.Logger.Error(ex.Message);
                return StatusCode(500, "Internal Error At " + DateTime.Now.ToLongTimeString());
            }
        }
        [HttpGet]
        [BypassLogging]
        public IActionResult Ping()
        {
            return Ok("I'm alive!");
        }
    }
}

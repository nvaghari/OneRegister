using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneRegister.Api.MasterCard.Filters;
using OneRegister.Api.Service.Abstract.Services;
using OneRegister.Core.Model.ControllerResponse;
using System;
using System.Threading.Tasks;

namespace OneRegister.Api.MasterCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(TokenAuthorizationAttribute))]
    public class InquiryController : ControllerBase
    {
        private readonly IInquiryService _inquiryService;
        private readonly ILogger<InquiryController> _logger;

        public InquiryController(IInquiryService inquiryService, ILogger<InquiryController> logger)
        {
            _inquiryService = inquiryService;
            _logger = logger;
        }
        /// <summary>
        /// update the status of the inquiry task and change it to be in Inprogress state. It means this inquiry will be run in the next iteration. 
        /// </summary>
        /// <param name="refid">this Id comes from GEMS database and most likely should be equivalent of CDDActionID</param>
        /// <param name="inquiryType">at the moment all posible values for Inquiry Type are: RPP,DV,SS or IV</param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> Patch(string refid, string inquiryType)
        {
            try
            {
                _logger.LogDebug($"<- Inquery patch request. refid: {refid} type: {inquiryType}");
                var taskId = await _inquiryService.ChangeStatusToInProgress(refid, inquiryType);
                _logger.LogDebug($"-> Inquery patch was successful. taskid: {taskId}");
                return Ok(taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"inquiry patch error: {ex.Message}");
                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }
        }
    }
}

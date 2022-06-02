using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneRegister.Api.Service.Abstract.Authorization;
using OneRegister.Api.Service.Exceptions;
using OneRegister.Core.Model.ControllerResponse;
using System;
using System.Threading.Tasks;

namespace OneRegister.Api.MasterCard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(IAuthorizationService authorizationService, ILogger<TokenController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }

        /// <summary>
        /// before each request you need to get and provide a token and send this token as Bearer Authentication in further requests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogDebug($"<- TokenRequest username: {Request.Headers["username"]}");
                if (Request.Headers.Keys.Contains("username") && Request.Headers.Keys.Contains("userkey"))
                {
                    var userName = Request.Headers["username"].ToString();
                    var userKey = Request.Headers["userkey"].ToString();
                    var token = await _authorizationService.GetTokenAsync(userName, userKey);
                    Response.Headers.Add("token", token);
                    _logger.LogDebug($"-> token: {token}");
                    return Ok();
                }
                else
                {
                    _logger.LogError("-> UserName Or UserKey is not provided.");
                    return BadRequest(SimpleResponse.FailBecause("UserName Or UserKey is not provided."));
                }
            }
            catch (AuthorizationException ex)
{
                _logger.LogError($"-> Unauthorized: {ex.Message}");
                return Unauthorized(SimpleResponse.FailBecause(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"token request error: {ex.Message}");
                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }

        }
    }
}

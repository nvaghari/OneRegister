using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace OneRegister.Web.Services.Audit
{
    public class WebhookAuditAttribute : Attribute,IActionFilter
    {
        private readonly WebhookLogService _webhookLogService;

        public WebhookAuditAttribute(WebhookLogService webhookLogService)
        {
            _webhookLogService = webhookLogService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            context.ActionArguments.TryGetValue("model", out object model);
            var content = JsonSerializer.Serialize(model);
            var logText = new StringBuilder();
            logText.Append("[<-] " + descriptor.ActionName);
            logText.Append(" " + context.HttpContext.Request.Method);
            logText.Append(" " + context.HttpContext.Request.ContentType);
            logText.Append(" " + context.HttpContext.Request.QueryString);
            logText.Append(" " + content);
            if (context.ActionDescriptor.EndpointMetadata.OfType<BypassLoggingAttribute>().Any())
            {
                _webhookLogService.Logger.Debug(logText.ToString());
            }
            else
            {
                _webhookLogService.Logger.Information(logText.ToString());
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var objectResult = context.Result as ObjectResult;
            var logText = new StringBuilder();
            logText.Append("[->] " + descriptor.ActionName);
            logText.Append(" " + context.HttpContext.Response.StatusCode);
            logText.Append(" " + context.HttpContext.Response.ContentType);
            logText.Append(" " + objectResult?.Value);
            if (context.ActionDescriptor.EndpointMetadata.OfType<BypassLoggingAttribute>().Any())
            {
                _webhookLogService.Logger.Debug(logText.ToString());
            }
            else
            {
                _webhookLogService.Logger.Information(logText.ToString());
            }
        }

    }
}

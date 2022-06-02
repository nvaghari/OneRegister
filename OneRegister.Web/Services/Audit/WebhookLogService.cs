using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OneRegister.Web.Models.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace OneRegister.Web.Services.Audit
{
    public class WebhookLogService
    {
        private readonly SerilogConfigModel _configuration;

        public WebhookLogService(
            IOptions<SerilogConfigModel> configuration)
        {
            _configuration = configuration.Value;
            Logger = CreateWebhookLog();
        }

        private ILogger CreateWebhookLog()
        {
            if (_configuration is null)
            {
                throw new System.Exception("Log Configuration Section is not available in appsettings.json");
            }
            var logConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(_configuration.Path + "OneRegisterWebhook.log", rollingInterval: RollingInterval.Day, outputTemplate: _configuration.Format);
            logConfig = SetDefaultLevel(_configuration, logConfig);
            return logConfig.CreateLogger();
        }

        public ILogger Logger { get; }

        private static LoggerConfiguration SetDefaultLevel(SerilogConfigModel serilogConfig, LoggerConfiguration logger)
        {
            logger = LogLevel(serilogConfig.AuditLevel.Default) switch
            {
                LogEventLevel.Verbose => logger.MinimumLevel.Verbose(),
                LogEventLevel.Debug => logger.MinimumLevel.Debug(),
                LogEventLevel.Information => logger.MinimumLevel.Information(),
                LogEventLevel.Warning => logger.MinimumLevel.Warning(),
                LogEventLevel.Error => logger.MinimumLevel.Error(),
                LogEventLevel.Fatal => logger.MinimumLevel.Fatal(),
                _ => logger.MinimumLevel.Information(),
            };
            return logger;
        }
        private static LogEventLevel LogLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return LogEventLevel.Information;
            var isValid = Enum.TryParse(typeof(LogEventLevel), level, true, out object result);
            if (!isValid) return LogEventLevel.Information;
            return (LogEventLevel)result;
        }
    }
}

using Microsoft.Extensions.Configuration;
using OneRegister.Web.Models.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace OneRegister.Web.Services.Setup
{
    public class SerilogConfiguration
    {
        public static void Initialize()
        {

            SerilogConfigModel serilogConfig = GetConfigs();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogLevel(serilogConfig.AuditLevel.Microsoft))
                .MinimumLevel.Override("Serilog", LogLevel(serilogConfig.AuditLevel.Serilog))
                .MinimumLevel.Override("Quartz", LogLevel(serilogConfig.AuditLevel.Quartz))
                .Enrich.FromLogContext()
                .WriteTo.File(serilogConfig.Path + serilogConfig.FileName, rollingInterval: RollingInterval.Day, outputTemplate: serilogConfig.Format);

            logger = SetDefaultLevel(serilogConfig, logger);
            Log.Logger = logger.CreateLogger();
        }

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

        private static SerilogConfigModel GetConfigs()
        {
            string configFilePath = "appsettings.json";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                configFilePath = "appsettings.Development.json";
            }
            var config = new ConfigurationBuilder()
            .AddJsonFile(configFilePath, optional: false)
            .Build();
            var model = config.GetSection("Services:Serilog").Get<SerilogConfigModel>();
            if (model is null)
            {
                throw new Exception("Log Configuration Section is not available in appsettings.json");
            }
            return model;
        }
    }
}

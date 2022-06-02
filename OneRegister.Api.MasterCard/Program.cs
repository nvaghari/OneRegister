using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OneRegister.Api.MasterCard.Model;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Api.MasterCard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SerilogInit();
            try
            {
                Log.Information("********** OneRegister API MasterCard Starting Up  **********");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "########## Application start-up failed ##########");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SerilogInit()
        {

            SerilogConfigModel serilogConfig = GetConfigs();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogLevel(serilogConfig.AuditLevel.Microsoft))
                .MinimumLevel.Override("Serilog", LogLevel(serilogConfig.AuditLevel.Serilog))
                .Enrich.FromLogContext()
                .WriteTo.File(serilogConfig.Path + serilogConfig.FileName, rollingInterval: RollingInterval.Day, outputTemplate: serilogConfig.Format);

            logger = SetDefaultLevel(serilogConfig, logger);
            Log.Logger = logger.CreateLogger();
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
            var model = config.GetSection("Serilog").Get<SerilogConfigModel>();
            if (model is null)
            {
                throw new System.Exception("Log Configuration Section is not available in appsettings.json");
            }
            return model;
        }
        private static LogEventLevel LogLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return LogEventLevel.Information;
            var isValid = Enum.TryParse(typeof(LogEventLevel), level, true, out object result);
            if (!isValid) return LogEventLevel.Information;
            return (LogEventLevel)result;
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
    }
}

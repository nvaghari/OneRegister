using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OneRegister.Web.Models.Configuration;
using OneRegister.Web.Services.Setup;
using Serilog;
using Serilog.Events;

namespace OneRegister.Web
{
    public class Program
    {

        public static void Main(string[] args)
        {
            SerilogConfiguration.Initialize();
            try
            {
                Log.Information("********** OneRegister Starting Up  **********");
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
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
    }
}

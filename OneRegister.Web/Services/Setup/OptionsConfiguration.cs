using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneRegister.Web.Models.Configuration;

namespace OneRegister.Web.Services.Setup
{
    public static class OptionsConfiguration
    {
        public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SerilogConfigModel>(configuration.GetSection(SerilogConfigModel.Position));

            return services;
        }

    }
}

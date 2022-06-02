using Microsoft.Extensions.DependencyInjection;
using OneRegister.Domain.Services.Startup;
using System;

namespace OneRegister.Web.Services.Setup
{
    public class DataSeeding
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
            seedingService.SeedOrganization();
            seedingService.SeedRoles();
            seedingService.SeedUsers();
        }
    }
}

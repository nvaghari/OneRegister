using Microsoft.Extensions.DependencyInjection;
using OneRegister.Data.Repository.Authentication;
using OneRegister.Security.Contract;
using OneRegister.Security.Services;
using OneRegister.Security.Services.Collecting;
using OneRegister.Security.Services.HttpContext;

namespace OneRegister.Web.Services.Dependency;

public static class SecurityModuleInjection
{
    public static void RegisterSecurityModule(this IServiceCollection services)
    {
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPermissionCollector, PermissionCollector>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IHttpContextPermissionHandler, HttpContextPermissionHandler>();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OneRegister.Data.Context;
using OneRegister.Data.Identication;

namespace OneRegister.Web.Services.Dependency;

public static class IdentityInjection
{
    public static void RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<OUser, ORole>()
                .AddEntityFrameworkStores<OneRegisterContext>()
                .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 7;
            options.Password.RequireUppercase = true;
        });
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OneRegister.Web.Services.Setup;

public static class CookiesConfiguration
{
    public static void ConfigureCookies(this IServiceCollection services, IConfiguration configuration)
    {
        var timeOut = configuration.GetValue<int>("CookieExpiryInMinutes");

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(timeOut);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;

            options.LoginPath = "/Account/ReLogin";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
        });
    }
}

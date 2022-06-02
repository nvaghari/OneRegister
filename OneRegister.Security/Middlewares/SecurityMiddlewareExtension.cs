using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OneRegister.Security.Middlewares;
public static class SecurityMiddlewareExtension
{
    public static IApplicationBuilder UseSecurityModule(this IApplicationBuilder builder,string forbiddenPageUrl)
    {
        return builder.UseMiddleware<SecurityMiddleware>(forbiddenPageUrl);
    }
}

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _forbiddenPageUrl;

    public SecurityMiddleware(RequestDelegate next,string forbiddenPageUrl)
    {
        _next = next;
        _forbiddenPageUrl = forbiddenPageUrl;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if (context.Response.StatusCode == 401)
        {
            context.Response.Redirect(_forbiddenPageUrl);
        }
    }
}


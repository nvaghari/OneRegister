using Microsoft.AspNetCore.Http;
using OneRegister.Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OneRegister.Security.Services.HttpContext;

public class HttpContextPermissionHandler : IHttpContextPermissionHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextPermissionHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid? UserId()
    {
        var identifier = _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault();
        if (string.IsNullOrEmpty(identifier)) return null;
        if (!Guid.TryParse(identifier, out Guid id)) throw new SecurityModuleException("the NameIdentifier in the claim is not a Guid");

        return id;
    }

    public string UserOrganizationPath()
    {
        return _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.UserData)
            .Select(c => c.Value)
            .FirstOrDefault();
    }

    public List<string> UserRoles()
    {
        return _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
    }
}

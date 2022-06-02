using System;
using System.Collections.Generic;

namespace OneRegister.Security.Services.HttpContext;

public interface IHttpContextPermissionHandler
{

    Guid? UserId();
    List<string> UserRoles();
    string UserOrganizationPath();
}

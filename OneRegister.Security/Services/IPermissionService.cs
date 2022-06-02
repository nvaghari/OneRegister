using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OneRegister.Security.Services
{
    public interface IPermissionService
    {
        void SynchronisePermissions(string assemblyName);
        bool IsAuthenticated(Guid userId, Guid permissionId);
        bool IsAuthenticated(string userName, Guid permissionId);
        bool IsAuthenticated(ClaimsPrincipal User, Guid permissionId);
        List<Guid> GetAuthorizedMenus(ClaimsPrincipal user);
        List<string> GetUserRoles(ClaimsPrincipal user);

        Dictionary<Guid, PermissionAttibuteModel> GetAddPermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions);
        Dictionary<Guid, PermissionAttibuteModel> GetUpdatePermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions);
        Dictionary<Guid, PermissionAttibuteModel> GetDeletePermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions);
    }
}
using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Contract;

public interface IPermissionRepository
{
    Dictionary<Guid, PermissionAttibuteModel> RetrievePermissions();
    void AddPermissions(Dictionary<Guid, PermissionAttibuteModel> permissions);
    void UpdatePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions);
    void DeletePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions);

    bool IsUserAuthorised(Guid userId, Guid permissionId);
    bool IsUserAuthorised(ClaimsPrincipal user, Guid permissionId);
    List<Guid> RetrieveMenus(List<string> userRoleNames);
    bool IsSuperAdmin(ClaimsPrincipal user);
    void RefreshInMemoryRolePermissions();
}

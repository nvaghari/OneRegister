using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Data.Identication;
using OneRegister.Security.Attributes;
using OneRegister.Security.Contract;
using OneRegister.Security.Exceptions;
using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Repository.Authentication
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly OneRegisterContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string _rolePermissionMemoryKey = "RolesPermissions";
        private Guid _superAdminId = Guid.Empty;
        private const string _superAdminKey = "SuperAdmin";


        private List<RolePemissionCacheModel> _rolePermissions = new();
        public List<RolePemissionCacheModel> RolePermissions
        {
            get
            {
                if (_rolePermissions.Count > 0)
                {
                    return _rolePermissions;
                }

                InitPermissionsIntoMemoryCache();
                return _rolePermissions;
            }
        }

        public PermissionRepository(
            OneRegisterContext context,
            IMemoryCache memoryCache
            )
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        #region MemoryCache

        private void InitPermissionsIntoMemoryCache()
        {
            LoadIntoMemoryRolePermissions();
            LoadIntoMemorySuperAdminRole();
        }

        private void LoadIntoMemorySuperAdminRole()
        {
            if (_memoryCache.TryGetValue(_superAdminKey, out Guid superAdminId))
            {
                _superAdminId = superAdminId;
            }
            else
            {
                var superAdmin = _context.Roles.FirstOrDefault(r => r.Name == _superAdminKey);
                if (superAdmin == null)
                {
                    throw new SecurityModuleException("SuperAdmin Role doesn't exist");
                }

                _memoryCache.Set(_superAdminKey, superAdmin.Id);
                _superAdminId = superAdmin.Id;
            }
        }

        private void LoadIntoMemoryRolePermissions()
        {
            if (_memoryCache.TryGetValue(_rolePermissionMemoryKey, out List<RolePemissionCacheModel> RolePermissions))
            {
                _rolePermissions = RolePermissions;
            }
            else
            {
                var rolePermissions = _context.Roles
                    .SelectMany(r => r.Permissions,(role,permission) => new RolePemissionCacheModel {RoleId = role.Id, RoleName = role.Name, PermissionId = permission.Id, AttributeType = permission.AttributeType})
                    .ToList();
                _memoryCache.Set(_rolePermissionMemoryKey, rolePermissions);
                _rolePermissions = rolePermissions;
            }
        }

        public void RefreshInMemoryRolePermissions()
        {
            if(_memoryCache.TryGetValue(_rolePermissionMemoryKey, out _))
            {
                var rolePermissions = _context.Roles
                    .SelectMany(r => r.Permissions, (role, permission) => new RolePemissionCacheModel { RoleId = role.Id, RoleName = role.Name, PermissionId = permission.Id, AttributeType = permission.AttributeType })
                    .ToList();
                _memoryCache.Set(_rolePermissionMemoryKey, rolePermissions);
                _rolePermissions = rolePermissions;
            }
        }

        #endregion MemoryCache

        #region CRUD Permissions

        public Dictionary<Guid, PermissionAttibuteModel> RetrievePermissions()
        {
            return _context.Permissions
                .Include(c => c.Organization)
                .ToDictionary(c => c.Id, c => new PermissionAttibuteModel
                {
                    Id = c.Id.ToString(),
                    ClassName = c.ClassName,
                    OrganizationId = c.OrganizationId.ToString(),
                    MethodName = c.MethodName,
                    Name = c.Name,
                    DomainName = c.Organization.Name
                });
        }

        public void UpdatePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            var claimsToUpdate = _context.Permissions.Where(c => permissions.Keys.Contains(c.Id));
            foreach (var claim in claimsToUpdate)
            {
                var updatedClaim = permissions[claim.Id];
                claim.Name = updatedClaim.Name;
                claim.OrganizationId = updatedClaim.OrganizationGuid;
                claim.ClassName = updatedClaim.ClassName;
                claim.MethodName = updatedClaim.MethodName;
                claim.AttributeType = updatedClaim.AttributeType;
                claim.ModifiedAt = DateTime.Now;
            }
            _context.Permissions.UpdateRange(claimsToUpdate);
            _context.SaveChanges();
        }

        public void AddPermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            var ct = DateTime.Now;
            var permissionList = permissions.Select(p => new Permission
            {
                Id = p.Value.Guid,
                Name = p.Value.Name,
                ClassName = p.Value.ClassName,
                MethodName = p.Value.MethodName,
                OrganizationId = p.Value.OrganizationGuid,
                AttributeType = p.Value.AttributeType,
                CreatedAt = ct,
                ModifiedAt = ct,
                CreatedBy = BasicUser.AdminId,
                ModifiedBy = BasicUser.AdminId,
                State = StateOfEntity.InProgress
            });
            _context.Permissions.AddRange(permissionList);
            _context.SaveChanges();
        }

        public void DeletePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            //TODO cascade delete for RoleClaims and UserClaims
            var claimsToDelete = _context.Permissions.Where(c => permissions.Keys.Contains(c.Id));
            _context.Permissions.RemoveRange(claimsToDelete);
            _context.SaveChanges();
        }

        #endregion CRUD Permissions

        #region Authorization

        public bool IsUserAuthorised(Guid userId, Guid permissionId)
        {
            var roleIds = _context
                .UserRoles.AsNoTracking()
                .Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList();

            if (IsUserSuperAdmin(roleIds)) return true;

            foreach (var roleId in roleIds)
            {
                if (_rolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == permissionId))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsUserSuperAdmin(List<Guid> roleIds)
        {
            return roleIds.Any(r => r == _superAdminId);
        }

        public List<Guid> RetrieveMenus(List<string> userRoleNames)
        {
            if(userRoleNames == null || userRoleNames.Count == 0)
            {
                return new List<Guid>();
            }
            var roles = RolePermissions
                .Where(r=> userRoleNames.Contains(r.RoleName))
                .Select(r=> r.RoleId)
                .ToList();
            return _rolePermissions
                .Where(rp =>rp.AttributeType == nameof(MenuAttribute) && roles.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId).ToList();
        }

        public bool IsSuperAdmin(ClaimsPrincipal user)
        {
            var userRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            return userRoles.Contains(BasicRoles.SuperAdmin.name);
        }

        public bool IsUserAuthorised(ClaimsPrincipal user, Guid permissionId)
        {
            if (IsSuperAdmin(user)) return true;
            List<string> userRoleClaims = GetUserRoleClaims(user);

            var rolePermission = RolePermissions.Where(rp => userRoleClaims.Contains(rp.RoleName));
            return rolePermission.Any(rp => rp.PermissionId == permissionId);
        }

        private List<string> GetUserRoleClaims(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        #endregion Authorization
    }
}
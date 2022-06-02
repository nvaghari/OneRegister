using Microsoft.AspNetCore.Identity;
using OneRegister.Data.Identication;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Account;
using OneRegister.Core.Model.ControllerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;
using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Contract;
using OneRegister.Security.Contract;
using OneRegister.Domain.Model.General;

namespace OneRegister.Domain.Services.Account
{
    public class RoleService
    {
        private readonly IOrganizedRepository<ORole> _roleRepository;
        private readonly UserService _userService;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(IOrganizedRepository<ORole> roleRepository,
            UserService userService,
            IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _userService = userService;
            _permissionRepository = permissionRepository;
        }

        public List<RoleListModel> GetRolesList()
        {
            return _roleRepository.GetList(true)
                .Select(r => new RoleListModel
                {
                    Id = r.Id,
                    AssignedNumber = GetAssignedUsers(r.Id).Count(),
                    Name = r.Name,
                    Organization = r.Organization.Name
                }).ToList();
        }
        private IEnumerable<OUser> GetAssignedUsers(Guid roleId)
        {
            var usersIds = _roleRepository.Context.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId);
            return _roleRepository.Context.Users.AsNoTracking().Where(u => usersIds.Contains(u.Id));
        }
        public void Add(ORole role)
        {
            _roleRepository.Add(role);
        }
        public FullResponse Remove(Guid roleId)
        {
            try
            {
                var role = _roleRepository.GetById(roleId);
                if (role is null)
                {
                    throw new ApplicationException("Role doesn't exist or you don't have access to it");
                }
                if (role.IsSystemic)
                {
                    throw new ApplicationException("The Role is Systemic and can not be removed");
                }
                if (GetAssignedUsers(roleId).Any())
                {
                    throw new ApplicationException("This Role is assigned to users and isn't possible to remove");
                }

                _roleRepository.Remove(role.Id);
                return FullResponse.Success;
            }
            catch (Exception ex)
            {

                return ex.ToFullResponse();
            }

        }

        public List<RoleManagementListModel> GetUserRoles(Guid userId)
        {
            var assignedRoles = _roleRepository.Context.UserRoles
                .Where(ur => ur.UserId == userId)
                .AsNoTracking()
                .Select(ur => ur.RoleId)
                .ToList();
            var assignedRole = _roleRepository.FilteredEntities
                .AsNoTracking()
                .Where(r => assignedRoles.Contains(r.Id))
                .ToList();
            return _roleRepository.GetList(true)
                .Select(r => new RoleManagementListModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Organization = r.Organization.Name,
                    IsAssigned = assignedRole.Any(a => a.Id == r.Id)
                })
                .ToList();
        }

        public List<GijgoTreeNode> GetUserRoleTree(Guid userId)
        {
            var result = new List<GijgoTreeNode>();
            var roleOrganizationNames = _roleRepository.Entities
                .AsNoTracking()
                .Include(r => r.Organization)
                .Select(r => r.Organization.Name)
                .ToList();
            var roleOrganizationGroups = roleOrganizationNames
                .GroupBy(r => r)
                .ToList();
            var assignedRoles = _roleRepository.Context.UserRoles
                .Where(ur => ur.UserId == userId)
                .AsNoTracking()
                .Select(ur => ur.RoleId)
                .ToList();

            foreach (var org in roleOrganizationGroups.OrderBy(o => o.Key))
            {
                result.Add(new()
                {
                    Id = Guid.Empty,
                    Text = org.Key,
                    Children = GetRolesUnderOrganization(org.Key, assignedRoles)
                });
            }

            return result;
        }

        private List<GijgoTreeNode> GetRolesUnderOrganization(string organizationName, List<Guid> assignedRoles)
        {
            return _roleRepository.Entities
                .Include(r => r.Organization)
                .Where(r => r.Organization.Name == organizationName)
                .OrderBy(r => r.Name)
                .Select(r => new GijgoTreeNode()
                {
                    Id = r.Id,
                    Text = r.Name,
                    Checked = assignedRoles.Contains(r.Id)
                })
                .ToList();
        }

        public List<OUser> UsersInRole(Guid roleId)
        {
            var userIds = _roleRepository.Context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.RoleId == roleId)
                .Select(ur => ur.UserId)
                .ToList();
            return _roleRepository.Context.Users.AsNoTracking().Where(u => userIds.Contains(u.Id)).ToList();
        }

        public FullResponse UpdateRolePermissions(Guid roleId, List<Guid> permissions)
        {
            try
            {
                permissions.RemoveAll(p => p == Guid.Empty);
                var newPermissions = _roleRepository.Context.Permissions.Where(p => permissions.Contains(p.Id)).ToList();
                var currentRole = _roleRepository.GetById(roleId, false, r => r.Permissions);
                var oldPermissions = currentRole.Permissions.ToList();

                var removeOne = oldPermissions.Except(newPermissions).ToList();
                var addOne = newPermissions.Except(oldPermissions).ToList();

                removeOne.ForEach(p => currentRole.Permissions.Remove(p));
                addOne.ForEach(p => currentRole.Permissions.Add(p));

                _roleRepository.Update(currentRole);
                _permissionRepository.RefreshInMemoryRolePermissions();
                return FullResponse.Success;
            }
            catch (Exception ex)
            {

                return ex.ToFullResponse();
            }
        }

        internal void SeedingAdd(ORole oRole)
        {
            oRole.CreatedBy = oRole.ModifiedBy = BasicUser.AdminId;
            if (oRole.State == StateOfEntity.Init)
            {
                oRole.State = StateOfEntity.InProgress;
            }
            _roleRepository.Add(oRole);
        }

        public FullResponse UpdateUserRoles(Guid userId, List<Guid> roles)
        {
            try
            {
                var comingRoleFromTree = _roleRepository.Entities
                    .Where(r => roles.Contains(r.Id)).ToList();

                var assignedRolesInDatabse = GetRoles(userId);
                var removeOnes = assignedRolesInDatabse.Except(comingRoleFromTree).ToList();
                var addOnes = comingRoleFromTree.Except(assignedRolesInDatabse).ToList();
                var user = _userService.GetById(userId);
                if (user.Id == BasicUser.AdminId && removeOnes.Any(r => r.Name == BasicRoles.SuperAdmin.name))
                {
                    throw new ApplicationException("You can't get Administrative role from Admin!");
                }
                RemoveRange(user, removeOnes);
                AddRange(user, addOnes);

                return FullResponse.Success;
            }
            catch (Exception ex)
            {

                return ex.ToFullResponse();
            }
        }
        private void RemoveRange(OUser user, List<ORole> roles)
        {
            foreach (var role in roles)
            {
                _roleRepository.Context.UserRoles.Remove(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = role.Id });
            }
            _roleRepository.Context.SaveChanges();
        }
        private void AddRange(OUser user, List<ORole> roles)
        {
            foreach (var role in roles)
            {
                _roleRepository.Context.UserRoles.Add(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = role.Id });
            }
            _roleRepository.Context.SaveChanges();
        }
        private List<ORole> GetRoles(Guid userId)
        {
            var assignedRoles = _roleRepository.Context.UserRoles
                .Where(ur => ur.UserId == userId)
                .AsNoTracking()
                .Select(ur => ur.RoleId)
                .ToList();
            return _roleRepository.FilteredEntities
                .Include(r => r.Organization)
                .AsNoTracking()
                .Where(r => assignedRoles.Contains(r.Id))
                .ToList();
        }
        public bool Any(string name, Guid orgId)
        {
            return _roleRepository.Entities.Any(r => r.Name == name.Trim() && r.OrganizationId == orgId);
        }
    }
}

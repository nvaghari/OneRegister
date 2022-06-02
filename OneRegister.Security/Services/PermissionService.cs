using OneRegister.Security.Comparer;
using OneRegister.Security.Contract;
using OneRegister.Security.Model;
using OneRegister.Security.Services.Collecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionCollector _permissionCollector;

        public PermissionService(
            IPermissionRepository permissionRepository,
            IPermissionCollector permissionCollector)
        {
            _permissionRepository = permissionRepository;
            _permissionCollector = permissionCollector;
        }

        public void SynchronisePermissions(string assemblyName)
        {
            try
            {
                if (string.IsNullOrEmpty(assemblyName))
                {
                    throw new ArgumentNullException(nameof(assemblyName), "assemblyName cant be empty");
                }
                var assemblyPermissions = _permissionCollector.CollectMethodAttributes(assemblyName);
                var collectedPermissions = _permissionCollector.ValidatePermissionCollection(assemblyPermissions);

                var repoPermissions = _permissionRepository.RetrievePermissions();

                var toAddPermissions = GetAddPermissions(collectedPermissions, repoPermissions);
                var toDeletePermissions = GetDeletePermissions(collectedPermissions, repoPermissions);
                var toUpdatePermissions = GetUpdatePermissions(collectedPermissions, repoPermissions);

                _permissionRepository.AddPermissions(toAddPermissions);
                _permissionRepository.DeletePermissions(toDeletePermissions);
                _permissionRepository.UpdatePermissions(toUpdatePermissions);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public Dictionary<Guid, PermissionAttibuteModel> GetUpdatePermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions)
        {
            return collectedPermissions
                .Intersect(repoPermissions, new PermissionModelComparer<Guid,PermissionAttibuteModel>())
                .Join(repoPermissions,c=>c.Key,r=>r.Key, (c,r)=> new {Key = c.Key, Value = c.Value, OldValue = r.Value})
                .Where(p => p.Value != p.OldValue)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public Dictionary<Guid, PermissionAttibuteModel> GetDeletePermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions)
        {
            return repoPermissions
                .Except(collectedPermissions, new PermissionModelComparer<Guid, PermissionAttibuteModel>())
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public Dictionary<Guid, PermissionAttibuteModel> GetAddPermissions(Dictionary<Guid, PermissionAttibuteModel> collectedPermissions, Dictionary<Guid, PermissionAttibuteModel> repoPermissions)
        {
            return collectedPermissions
                .Except(repoPermissions, new PermissionModelComparer<Guid, PermissionAttibuteModel>())
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public bool IsAuthenticated(Guid userId, Guid permissionId)
        {
            return _permissionRepository.IsUserAuthorised(userId, permissionId);
        }

        public bool IsAuthenticated(string userName, Guid permissionId)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated(ClaimsPrincipal User, Guid permissionId)
        {
            if (User == null || User.Claims == null) return false;

            return _permissionRepository.IsUserAuthorised(User, permissionId);
        }

        public List<Guid> GetAuthorizedMenus(ClaimsPrincipal user)
        {
            var userRoleNames = GetUserRoles(user);
            return _permissionRepository.RetrieveMenus(userRoleNames);
        }

        public List<string> GetUserRoles(ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}

using OneRegister.Security.Contract;
using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Security.Test.Mocks
{
    internal class MockPermissionRepository : IPermissionRepository
    {
        private Dictionary<Guid, PermissionAttibuteModel> _localDb;
        public MockPermissionRepository()
        {
            _localDb = new();
            _localDb.Add(Guid.Parse("89B6D466-F95F-4F4B-970E-E04C60556B2C"),new PermissionAttibuteModel() 
            {Id = "89B6D466-F95F-4F4B-970E-E04C60556B2C", 
                Name = "View Register Page",
                OrganizationId = BasicOrganizations.MasterCard_ID,
                MethodName = "Register",
                ClassName = "MasterCardController"
            });
            _localDb.Add(Guid.Parse("75EECCB8-6FA4-45FD-9E7C-6F12D6919BAF"), new PermissionAttibuteModel()
            {
                Id = "75EECCB8-6FA4-45FD-9E7C-6F12D6919BAF",
                Name = "something to remove",
                OrganizationId = BasicOrganizations.MasterCard_ID,
                MethodName = "OldMethod",
                ClassName = "MasterCardController"
            });
        }
        public void AddPermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            throw new NotImplementedException();
        }

        public void DeletePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            throw new NotImplementedException();
        }

        public bool IsSuperAdmin(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public bool IsUserAuthorised(Guid userId, Guid permissionId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserAuthorised(ClaimsPrincipal user, Guid permissionId)
        {
            throw new NotImplementedException();
        }

        public void RefreshInMemoryRolePermissions()
        {
            throw new NotImplementedException();
        }

        public List<Guid> RetrieveMenus(List<string> userRoleNames)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Guid, PermissionAttibuteModel> RetrievePermissions()
        {
            return _localDb;
        }

        public void UpdatePermissions(Dictionary<Guid, PermissionAttibuteModel> permissions)
        {
            throw new NotImplementedException();
        }
    }
}

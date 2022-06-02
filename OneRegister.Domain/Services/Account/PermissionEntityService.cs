using OneRegister.Data.Contract;
using OneRegister.Data.Identication;
using OneRegister.Data.Repository.Authentication;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.Account
{
    public class PermissionEntityService
    {
        private readonly IOrganizedRepository<Permission> _permissionEntityRepository;

        public PermissionEntityService(IOrganizedRepository<Permission> permissionEntityRepository)
        {
            _permissionEntityRepository = permissionEntityRepository;
        }
        public List<PermissionListModel> GetPermissionLists()
        {
            return _permissionEntityRepository.GetList(asNoTrack:true)
                            .Select(p => new PermissionListModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                AttributeType = p.AttributeType,
                                ClassName = p.ClassName,
                                MethodName = p.MethodName,
                                OrganizationName = p.Organization.Name
                            })
                            .ToList();

        }
        public List<GijgoTreeNode> GetPermissionTree(Guid roleId)
        {
            var currentPermissions = _permissionEntityRepository
                .Context.RolePermissions
                .Where(rp => rp.RolesId == roleId)
                .Select(rp=>rp.PermissionsId)
                .ToList();
            var result = new List<GijgoTreeNode>();
            var permissions = _permissionEntityRepository.Entities.Select(p=> p.AttributeType).ToList();
            var permissionTypeGroups = permissions.GroupBy(p=> p).ToList();
            foreach (var permissionTypeGroup in permissionTypeGroups.OrderBy(p=>p.Key))
            {
                result.Add(new() { 
                    Id = Guid.Empty,
                    Text = permissionTypeGroup.Key,
                    Children = GetChildsByAttributeType(permissionTypeGroup.Key, currentPermissions)
                });
            }
            return result;
        }

        private List<GijgoTreeNode> GetChildsByAttributeType(string attributeTypeName,List<Guid> currentPermissions)
        {
            var result = new List<GijgoTreeNode>();
            var permissions = _permissionEntityRepository.Entities
                .Where(p => p.AttributeType == attributeTypeName)
                .Select(p => p.ClassName).ToList();
            var classGroups = permissions.GroupBy(p => p).ToList();
            foreach (var group in classGroups.OrderBy(p=>p.Key))
            {
                result.Add(new()
                {
                    Id = Guid.Empty,
                    Text = group.Key,
                    Children = GetChildsByClassAndType(group.Key, attributeTypeName, currentPermissions)
                });
            }
            return result;
        }

        private List<GijgoTreeNode> GetChildsByClassAndType(string className,string attributeType, List<Guid> currentPermissions)
        {
            return _permissionEntityRepository.Entities
                .Where(p =>p.AttributeType == attributeType && p.ClassName == className)
                .OrderBy(p=>p.Name)
                .Select(p => new GijgoTreeNode
                {
                    Id = p.Id,
                    Text = p.Name,
                    Checked = currentPermissions.Contains(p.Id)
                })
                .ToList();
        }
    }
}

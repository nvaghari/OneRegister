using OneRegister.Data.Contract;
using OneRegister.Data.Identication;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.Shared;
using System;
using System.Collections.Generic;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.Startup
{
    public class SeedingService
    {
        private readonly OrganizationService _organizationService;
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public SeedingService(
            OrganizationService organizationService,
            UserService userService,
            RoleService roleService

            )
        {
            _organizationService = organizationService;
            _userService = userService;
            _roleService = roleService;
        }
        public void SeedOrganization()
        {
            var orgList = BasicOrganizations.GetList();
            foreach (var (id, name) in orgList)
            {
                if (_organizationService.Any(id))
                {
                    continue;
                }
                var organization = new Organization
                {
                    Name = name,
                    Id = id,
                    IsSystemic = true
                };
                if (id != BasicOrganizations.OneRegister)
                {
                    organization.ParentId = BasicOrganizations.OneRegister;
                }
                _organizationService.SeedingAdd(organization);
            }
        }
        public void SeedRoles()
        {
            var roleList = new List<(string name, Guid orgId)> {
                BasicRoles.SuperAdmin,
                BasicRoles.Merchant,
                BasicRoles.MerchantOPLvl1,
                BasicRoles.MerchantOPLvl2,
                BasicRoles.MerchantRiskHead,
                BasicRoles.SalesPerson
            };
            foreach (var (name, orgId) in roleList)
            {
                if (_roleService.Any(name, orgId))
                {
                    continue;
                }

                _roleService.SeedingAdd(new ORole
                {
                    Name = name,
                    OrganizationId = orgId,
                    IsSystemic = true
                });
            }
        }
        public void SeedUsers()
        {
            var adminUser = new OUser
            {
                Id = BasicUser.AdminId,
                OrganizationId = BasicOrganizations.OneRegister,
                Name = BasicUser.AdminName,
                Email = BasicUser.AdminEmail,
                PhoneNumber = BasicUser.AdminPhone,
                UserName = BasicUser.AdminUser,
                State = StateOfEntity.Complete,
                IsSystemic = true
            };

            if (!_userService.Any(BasicUser.AdminId))
            {
                _userService.SeedingAdd(adminUser, BasicUser.AdminPassword);
            }
            if (!_userService.IsInRole(adminUser, BasicRoles.SuperAdmin.name))
            {
                _userService.AddToRole(adminUser.Id, BasicRoles.SuperAdmin.name);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Data.Identication;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.Shared;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.Account;
using OneRegister.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly OrganizationService _organizationService;
        private readonly RoleService _roleService;
        private readonly PermissionEntityService _permissionEntityService;

        public RoleController(
            OrganizationService organizationService,
            RoleService roleService,
            PermissionEntityService permissionEntityService)
        {
            _organizationService = organizationService;
            _roleService = roleService;
            _permissionEntityService = permissionEntityService;
        }
        [HttpGet]
        [Menu(Id: "E9AA44AE-472D-4D1E-AE5F-E7F2A06A7DDA", Name: "Roles", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 4, Parent = "30EC2EE1-B3BE-457D-BFB0-B426F02C9224")]
        public IActionResult List()
        {
            RoleAddViewModel model = new RoleAddViewModel
            {
                Organizations = _organizationService.GetBaseOrganizationsDictionary()
            };
            return View(model);
        }

        #region API
        [HttpPost]
        public JsonResult AddRole(RoleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                ORole role = new() { Name = model.Name, OrganizationId = model.OrganizationId.Value };
                _roleService.Add(role);
                return Json(FullResponse.SuccessWithId(role.Id.ToString()));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }
        [HttpPost]
        public JsonResult GetRoles()
        {
            List<RoleListModel> model = _roleService.GetRolesList();
            return Json(model);
        }
        [HttpPost]
        public JsonResult RemoveRole(Guid roleId)
        {
            FullResponse result = _roleService.Remove(roleId);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetPermissionTree(Guid roleId)
        {
            var permissionTree = _permissionEntityService.GetPermissionTree(roleId);
            return Json(permissionTree);
        }
        [HttpPost]
        public JsonResult UpdateRolePermissions(Guid roleId,List<Guid> permissions)
        {
            FullResponse result = _roleService.UpdateRolePermissions(roleId, permissions);
            return Json(result);
        }
        [HttpPost]
        public JsonResult UsersInRole(Guid roleId)
        {
            List<OUser> users = _roleService.UsersInRole(roleId);
            var model = users.Select(u => new UsersInRoleViewModel{
                UserName = u.UserName,
                Name = u.Name,
                IsActive = u.State != Data.Contract.StateOfEntity.Pending
            }).ToList();
            return Json(model);
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Domain.Services.Account;
using OneRegister.Security.Attributes;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly PermissionEntityService _permissionEntityService;
        #region CTOR
        public PermissionController(PermissionEntityService permissionEntityService)
        {
            _permissionEntityService = permissionEntityService;
        }
        #endregion
        #region Entries
        [HttpGet]
        [Menu(Id: "12A091BF-5574-4F02-A643-E9811D9594EA", Name: "Permissions", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 5, Parent = "30EC2EE1-B3BE-457D-BFB0-B426F02C9224")]
        public IActionResult List()
        {
            return View();
        }
        #endregion
        #region API
        [HttpPost]
        public JsonResult GetPermissions()
        {
            var permissionLists = _permissionEntityService.GetPermissionLists();

            return Json(permissionLists);
        }
        #endregion
    }
}

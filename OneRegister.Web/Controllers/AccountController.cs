using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Data.Identication;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Model.General;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.Shared;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.Account;
using OneRegister.Web.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        public readonly RoleService _roleService;
        private readonly AuthorizationService _authorizationService;
        private readonly OrganizationService _organizationService;
        private readonly UserService _userService;
        public AccountController(
            OrganizationService organizationService,
            AuthorizationService authorizationService,
            UserService userService,
            RoleService roleService,
            IMapper mapper,
            ILogger<AccountController> logger)
        {
            _organizationService = organizationService;
            _authorizationService = authorizationService;
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }
        public IActionResult AccessDenied()
        {

            //TODO verbose log
            return View();
        }
        [HttpGet]
        [Menu(Id: "D9E176B3-3918-4671-ACB9-38B6FFF506CC", Name: "Users", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 3, Parent = "30EC2EE1-B3BE-457D-BFB0-B426F02C9224")]
        public IActionResult List()
        {
            var model = _userService.RetrieveUsers();
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogInformation("Login -> Page View");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                LoginModel model = _mapper.Map<LoginModel>(viewModel);
                var loginResult =await _authorizationService.Login(model);
                if (loginResult.IsSuccessful)
                {
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());
                }
                ModelState.AddModelError("Error", loginResult.Message);
                return View(viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("LogOut -> User: " + User.Identity.Name);
            await _authorizationService.LogoutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(AccountController.Login));
        }

        public IActionResult Profile()
        {
            var currentUser = _userService.GetByUserName(User.Identity.Name);
            var model = new UserProfileViewModel
            {
                UserId = currentUser.Id,
                Name = currentUser.Name,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber
            };
            return View(model);
        }
        [HttpGet]
        [Menu(Id: "4C39F690-954A-4540-AA2D-3EC8C710C71C", Name: "Create User", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 2, Parent = "30EC2EE1-B3BE-457D-BFB0-B426F02C9224")]
        public IActionResult Register()
        {
            var model = new UserRegisterViewModel();
            var user = _authorizationService.GetCurrentUser();
            var userOrganization = _organizationService.Get(user.OrganizationId);
            var userOrgNode = _organizationService.GetOrganizationHierarchy(new OrgNode
            {
                Id = userOrganization.Id,
                Level = 0,
                Name = userOrganization.Name
            });
            model.Organizations = _organizationService.ListDescendants(userOrgNode);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ReLogin()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult Unauthorize()
        {
            return View();
        }
        #region API
        

        [HttpPost]
        public JsonResult ChangePassword(Guid userId, string oldPassword, string newPassword, string confirm)
        {
            var model = new SimpleResponse();
            if (string.IsNullOrEmpty(newPassword))
            {
                model.IsSuccessful = false;
                model.Message = "New password can't be empty";
                return Json(model);
            }
            if (string.Compare(newPassword, confirm) != 0)
            {
                model.IsSuccessful = false;
                model.Message = "Password confirm is not match";
                return Json(model);
            }
            var currentUser = _userService.GetByUserName(User.Identity.Name);
            if (currentUser.Id != userId)
            {
                model.IsSuccessful = false;
                model.Message = "You can only change your own password!";
                return Json(model);
            }
            //var result = _authorizationService.ChangePasswordAsync(currentUser, oldPassword, newPassword).Result;
            var result = _userService.ChangePassword(currentUser, oldPassword, newPassword);
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUser(Guid userId)
        {
            try
            {
                _userService.DisableUser(userId);
                return Json(new SimpleResponse { IsSuccessful = true });
            }
            catch (Exception ex)
            {

                return Json(new SimpleResponse
                {
                    IsSuccessful = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        public JsonResult GetUserRoles(Guid userId)
        {
            List<RoleManagementListModel> result = _roleService.GetUserRoles(userId);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUserRoleTree(Guid userId)
        {
            var roleTree = _roleService.GetUserRoleTree(userId);
            return Json(roleTree);
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            List<UserListModel> model = _userService.RetrieveUsers();
            return Json(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public JsonResult Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRegisterModel user = _mapper.Map<UserRegisterModel>(model);
                FullResponse result = _userService.Create(user, User);
                return Json(result);
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        

        [HttpPost]
        public JsonResult ResetUser(Guid userId)
        {
            try
            {
                var user = _userService.GetById(userId);
                if (user.UserName.ToLower() == "admin")
                {
                    throw new Exception("is not possible to reset admin password");
                }
                var modifier = _authorizationService.GetCurrentUserId();
                var result = _userService.ResetPassword(user, modifier.Value);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }
        [HttpPost]
        public JsonResult RestoreUser(Guid userId)
        {
            try
            {
                _userService.EnableUser(userId);
                return Json(new SimpleResponse { IsSuccessful = true });
            }
            catch (Exception ex)
            {

                return Json(new SimpleResponse
                {
                    IsSuccessful = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost]
        public JsonResult UpdateUserRoles(Guid userId, List<Guid> roles)
        {
            FullResponse result = _roleService.UpdateUserRoles(userId, roles);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetOrganizations(Guid userId)
        {
            var currentUser = _authorizationService.GetCurrentUser();
            var currentUserOrganization = _organizationService.Get(currentUser.OrganizationId);
            var rootNode = _organizationService.GetOrganizationHierarchy(new OrgNode
            {
                Id = currentUser.OrganizationId,
                Name = currentUserOrganization.Name,
                Level = 0
            });
            var user = _userService.GetById(userId);
            List<NinoTreeViewModel> model = new();

            var treeNode = AddTreeNodes(rootNode, user.OrganizationId);
            model.Add(treeNode);
            return Json(model);
        }

        private NinoTreeViewModel AddTreeNodes(OrgNode orgNode, Guid userOrgId)
        {
            var treeNode = new NinoTreeViewModel
            {
                Id = orgNode.Id,
                Name = orgNode.Name,
                Selected = orgNode.Id == userOrgId
            };
            if (orgNode.Descendants.Any())
            {
                foreach (var child in orgNode.Descendants)
                {
                    treeNode.Childs.Add(
                        AddTreeNodes(child, userOrgId)
                    );
                }
            }
            return treeNode;
        }

        [HttpPost]
        [Permission(Id: "FC5916F6-2554-4CC8-88BC-1FD849279373", Name: "Change User Organization", OrganizationId: BasicOrganizations.OneRegister_ID)]
        public JsonResult ChangeUserOrganization(Guid userId, Guid orgId)
        {
            try
            {
                _userService.ChangeOrganization(userId, orgId);
                return Json(FullResponse.Success);
            }
            catch (Exception ex)
            {

                return Json(ex.ToFullResponse());
            }
        }
        #endregion
    }
}
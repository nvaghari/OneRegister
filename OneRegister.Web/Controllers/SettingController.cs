using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Notification;
using OneRegister.Domain.Model.Settings;
using OneRegister.Domain.Services.Email;
using OneRegister.Domain.Services.Settings;
using OneRegister.Domain.Services.Startup;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Security.Services;
using OneRegister.Web.Models.Settings;
using System;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id:"3EBBAB5F-60AB-4EE2-8246-966CE3B781B9",Name:"Settings",OrganizationId:BasicOrganizations.OneRegister_ID,Order =9,FasIcon = "fa-cogs")]
    public class SettingController : Controller
    {
        private readonly SettingService _settingService;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;
        private readonly CollectingEnumService _collectingEnumService;
        private readonly IPermissionService _permissionService;

        public SettingController(
            SettingService settingService, 
            IMapper mapper,
            EmailService emailService,
            CollectingEnumService collectingEnumService,
            IPermissionService permissionService)
        {
            _settingService = settingService;
            _mapper = mapper;
            _emailService = emailService;
            _collectingEnumService = collectingEnumService;
            _permissionService = permissionService;
        }
        [HttpGet]
        [Menu(Id: "1C786783-E6A7-4168-8847-34B46D729ADF", Name: "Email", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 2,Parent = "3EBBAB5F-60AB-4EE2-8246-966CE3B781B9")]
        public IActionResult Email()
        {
            MailSettingViewModel model = new();
            MailSettingModel domainModel = _settingService.GetEmail();
            if(domainModel == null)
            {
                model.UseAuthentication = true;
                model.SmtpPort = 25;
            }
            else
            {
                model = _mapper.Map<MailSettingViewModel>(domainModel);
            }

            return View(model);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult SaveEmail(MailSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailSettingModel domainModel = _mapper.Map<MailSettingModel>(model);
                    var id = _settingService.Save(domainModel);
                    return Json(FullResponse.SuccessWithId(id.ToString()));
                }
                else
                {
                    return Json(ModelState.FullResponse());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToFullResponse());
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult SendEmail(MailSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sendModel = _mapper.Map<SendEmailModel>(model);
                    sendModel.Subject = "Test Email";
                    _emailService.Send(sendModel);
                    return Json(FullResponse.Success);
                }
                else
                {
                    return Json(ModelState.FullResponse());
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToFullResponse());
            }
        }


        [HttpGet]
        [Menu(Id: "B0E583F8-52AC-47EC-98C6-2CFC24A73CEF", Name: "Application", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 1, Parent = "3EBBAB5F-60AB-4EE2-8246-966CE3B781B9")]
        public IActionResult Application()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SyncCodeList()
        {
            var result = _collectingEnumService.UpdateEnumList();
            return Json(result);
        }
        [HttpPost]
        public JsonResult SyncPermissions()
        {
            try
            {
                _permissionService.SynchronisePermissions("OneRegister.Web");
                return Json(SimpleResponse.Success("Permissions are synched"));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Contract;
using OneRegister.Domain.Services.KYCApi;
using OneRegister.Domain.Services.MasterCard;
using OneRegister.Domain.Services.MasterCard.Model;
using OneRegister.Domain.Services.RPPApi;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.MasterCard;
using System;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "B0C6221B-5104-4EDD-9487-C4CF736B4200", Name: "MasterCard", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 1,FasIcon = "fa-credit-card")]
    public class MasterCardController : Controller
    {
        private readonly MasterCardService _masterCardService;
        private readonly RPPService _rPPService;
        private readonly IMapper _mapper;
        private readonly KYCService _kYCService;
        public MasterCardController(
            MasterCardService masterCardService,
            RPPService rPPService,
            IMapper mapper,
            KYCService kYCService
            )
        {
            _masterCardService = masterCardService;
            _rPPService = rPPService;
            _mapper = mapper;
            _kYCService = kYCService;
        }
        [Menu(Id: "7C569778-1593-43AE-97D7-F73AD3E42944", Name: "Register", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 1, Parent = "B0C6221B-5104-4EDD-9487-C4CF736B4200")]
        [Permission(Id: "89B6D466-F95F-4F4B-970E-E04C60556B2C", Name: "View Register Page", OrganizationId: BasicOrganizations.MasterCard_ID)]
        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new();
            FillRegisterViewModelLists(model);
            model.ICSource = "DE1";
            model.Channel = "APPC";
            model.OrgID = new Guid("97BD9989-86F5-4CC0-8269-4754D382CE03");
            model.IsAddressSame = true;
            model.Nationality = "MY";
            model.HomeAddress.CountryCode = "MY";
            model.PostAddress.CountryCode = "MY";
            return View(model);
        }

        [HttpPost]
        [Permission(Id: "D4683477-35AB-4A7E-A0F0-19E9888F7CE5", Name: "Do Register", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainModel = _mapper.Map<MasterCardRegisterModel>(model);
                FullResponse result = _masterCardService.Register(domainModel);
                return Json(result);
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpGet]
        [Menu(Id: "606FF9F2-47E1-45CE-BCBE-F4DD032F4175", Name: "Customer List", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 2, Parent = "B0C6221B-5104-4EDD-9487-C4CF736B4200")]
        [Permission(Id: "543F3726-A2B8-4919-B087-04789976F2F5", Name: "View List Page", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        [Permission(Id: "311E1A9A-C7C5-4D83-922C-D142D1EEBFC5", Name: "View List", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult List(DtReceive dtReceive)
        {
            var model = _masterCardService.RetrieveForList(dtReceive);
            return Json(model);
        }

        [HttpGet]
        [Menu(Id: "00D1F5E2-6240-4CE3-B633-388BBA1A1B00", Name: "Check Bank Account Name", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 3, Parent = "B0C6221B-5104-4EDD-9487-C4CF736B4200")]
        [Permission(Id: "E0E8BB26-CEB0-4646-976B-0D7C0D87DDCD", Name: "View Check Account Page", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public IActionResult CheckAccount()
        {
            return View(new CheckAccountViewModel());
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Permission(Id: "0BA63B53-E1E7-4121-97BB-0944A5EE118E", Name: "Do Check Account", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult CheckAccount(CheckAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainModel = _mapper.Map<CheckBankAccountModel>(model);
                var checkResult = _rPPService.CheckBankAccount(domainModel);

                if (checkResult.IsSuccessful) return Json(FullResponse.Success);
                return Json(FullResponse.FailBecause(checkResult.RetMsg));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpGet]
        [Menu(Id: "11689D0E-8DB1-4E0E-84DA-FAA948B65731", Name: "Check Sanction Screening", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 4, Parent = "B0C6221B-5104-4EDD-9487-C4CF736B4200")]
        [Permission(Id: "4498635B-DF12-4A72-828E-AFFC9DCC8A60", Name: "View Check Sanction Screening Page", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public IActionResult CheckSanctionScreening()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Permission(Id: "CD05CC25-124F-4BFF-A050-4721B4283018", Name: "Do Check Sanction Screening", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult CheckSanction(CheckSanctionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _kYCService.CheckSanctionScreening(model.FirstName, model.LastName, model.BirthDay);
                    var successResult = new FullResponse()
                    {
                        IsSuccessful = true,
                        Message = result,
                    };
                    return Json(successResult);
                }
                catch (Exception ex)
                {

                    return Json(FullResponse.FailBecause(ex.Message));
                }
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpGet]
        [Permission(Id: "516DD3B0-0B7F-4DA5-8647-7649AA6623FC", Name: "View Task List Page", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public IActionResult TaskList()
        {
            return View();
        }


        [HttpPost]
        [Menu(Id: "CA56540A-DCF5-487A-92C1-48C5FAE378F4", Name: "TaskList", OrganizationId: BasicOrganizations.MasterCard_ID, Order = 5, Parent = "B0C6221B-5104-4EDD-9487-C4CF736B4200")]
        [Permission(Id: "12EDD091-2482-40E0-831F-91628798A027", Name: "View Task List", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult TaskList(DtReceive dtReceive)
        {
            DtReturn<MCTaskListGridModel> model = _masterCardService.RetrieveTasksForList(dtReceive);
            return Json(model);
        }


        [HttpPost]
        [Permission(Id: "E301F5B8-FEC4-47BD-982C-83D72025E413", Name: "Do Reset Task", OrganizationId: BasicOrganizations.MasterCard_ID)]
        public JsonResult ResetTask(Guid taskId)
        {
            try
            {
                _ = _masterCardService.ChangeInquiryTaskState(taskId, StateOfEntity.InProgress);
                return Json(SimpleResponse.Success());
            }
            catch (Exception ex)
            {

                return Json(SimpleResponse.FailBecause(ex.Message));
            }

        }
        private void FillRegisterViewModelLists(RegisterViewModel model)
        {
            model.ChannelList = _masterCardService.GetChannelList();
            model.NationalityList = _masterCardService.GetNationalityList();
            model.BankBICList = _masterCardService.GetBankBICList();
            model.OccIndustryList = _masterCardService.GetOccIndustryList();
            model.OccTypeList = _masterCardService.GetOccTypeList();
            model.ICSourceList = _masterCardService.GetIcSourceList();
            model.ListPackagesList = _masterCardService.GetListPackagesList();
            model.HomeAddress.CountryCodeList = model.PostAddress.CountryCodeList = _masterCardService.GetNationalityList();
            model.HomeAddress.StateCodeList = model.PostAddress.StateCodeList = _masterCardService.GetCountryStateList();
            model.HomeAddress.AddrTypeList = model.PostAddress.AddrTypeList = _masterCardService.GetAddressTypeList();

        }
    }
}

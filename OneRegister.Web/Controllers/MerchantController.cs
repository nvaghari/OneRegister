using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Data.Model.MerchantRegistration;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Domain.Model.Shared.Billboard;
using OneRegister.Domain.Services.MerchantRegistration;
using OneRegister.Domain.Services.Shared;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.MerchantRegistration;
using OneRegister.Web.Services.ViewService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "1DCB3F54-CEB0-412A-94B7-D9E6D9D1094A", Name: "Merchant", OrganizationId: BasicOrganizations.Merchant_ID, Order = 5, FasIcon = "fa-handshake")]
    public class MerchantController : Controller
    {
        private readonly MerchantService _merchantService;
        private readonly CodeListService _codeListService;
        private readonly IMapper _mapper;
        private readonly ViewRenderService _viewRenderService;

        public MerchantController(
            MerchantService merchantService,
            CodeListService codeListService,
            IMapper mapper,
            ViewRenderService viewRenderService)
        {
            _merchantService = merchantService;
            _codeListService = codeListService;
            _mapper = mapper;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        [Permission("3CA8C560-F9B0-4957-BE7E-F343702F10EB", "Edit Commission (View)", BasicOrganizations.Merchant_ID)]
        public IActionResult EditCommission(Guid mid)
        {
            MerchantCommissionModel domainModel = _merchantService.GetCommissionByMerchantId(mid);
            if (domainModel == null)
            {
                return Billboard(BillboardType.NotFound, "Merchant is not valid", "Merchant/List");
            }
            var model = _mapper.Map<MerchantCommissionViewModel>(domainModel);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Permission("3BDA36A4-1818-46CC-9D92-6B88AF407043", "Edit Commission", BasicOrganizations.Merchant_ID)]
        public JsonResult EditCommission(MerchantCommissionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var domainModel = _mapper.Map<MerchantCommissionModel>(model);
                    FullResponse result = _merchantService.UpdateCommission(domainModel);
                    return Json(result);
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
        [Route("Merchant/Register/GetCommission")]
        [Route("Merchant/GetCommission")]
        public async Task<IActionResult> GetCommission(Guid mid)
        {
            var domainModel = _merchantService.GetCommissionByMerchantId(mid);
            var model = _mapper.Map<MerchantCommissionViewModel>(domainModel);
            var viewResult = await _viewRenderService.RenderToStringAsync(ControllerContext, "_CommissionRead", model);
            return Content(viewResult);
        }
        [HttpGet]
        [Route("Merchant/Register/GetCommissionForPrint")]
        [Route("Merchant/GetCommissionForPrint")]
        public IActionResult GetCommissionForPrint(Guid mid)
        {
            var domainModel = _merchantService.GetCommissionByMerchantId(mid);
            var model = _mapper.Map<MerchantCommissionViewModel>(domainModel);
            return View("_CommissionRead", model);
        }

        [HttpGet]
        [Menu(Id: "AFC407C8-55AD-43A4-9392-976746D8DEDF", Name: "Inbox", OrganizationId: BasicOrganizations.Merchant_ID, Order = 1, Parent = "1DCB3F54-CEB0-412A-94B7-D9E6D9D1094A")]
        public ActionResult Inbox()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "FE32CB55-FEF3-4571-B4C1-D34C3D5975DA", Name: "List", OrganizationId: BasicOrganizations.Merchant_ID, Order = 2, Parent = "1DCB3F54-CEB0-412A-94B7-D9E6D9D1094A")]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "BB52EC3A-B6D0-4DFF-997B-2518E570B809", Name: "Register", OrganizationId: BasicOrganizations.Merchant_ID, Order = 3, Parent = "1DCB3F54-CEB0-412A-94B7-D9E6D9D1094A")]
        [InlinePermission("8B01A4A4-765E-407D-A6AD-684E92891737", "See Internal Merchant Status",BasicOrganizations.Merchant_ID)]
        [InlinePermission("93F95642-AB53-424D-8B70-A67F1965D267", "See Internal Reject Remark",BasicOrganizations.Merchant_ID)]
        public IActionResult Register(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                MerchantRegisterModel newModel = _merchantService.GetNewRegisterModelForCurrentUser();
                MerchantRegisterViewModel newViewModel = _mapper.Map<MerchantRegisterViewModel>(newModel);

                //default value requested by business
                newViewModel.Info.Country = "MY";

                return View(FillUpLists(newViewModel));
            }
            var model = _merchantService.GetMerchantInfoByFormNo(id);
            if (model == null)
            {
                return Billboard(BillboardType.NotFound, "Form Number is not valid", "Merchant/List");
            }
            MerchantRegisterViewModel viewModel = _mapper.Map<MerchantRegisterViewModel>(model);
            FillUpLists(viewModel);
            return View(viewModel);
        }

        private IActionResult Billboard(BillboardType type, string text, string returnUrl)
        {
            TempData["bText"] = text;
            TempData["bUrl"] = returnUrl;
            return RedirectToAction("Index", "Billboard", new { t = (int)type });
        }

        [AllowAnonymous]
        [Menu("9FE978C2-FD9E-4122-AA23-F26E22046543", "Register Merchant Account",BasicOrganizations.Merchant_ID,Order =1,Parent = "323DC892-64D6-4E85-8BC9-BAE0C335F215")]
        public ActionResult UserRegister()
        {
            return View();
        }

        #region API

        [HttpPost]
        [Route("Merchant/Register/Accept")]
        [Route("Merchant/Accept")]
        [Permission("893F73A2-0679-4D57-B20D-7CC14B6AAC88", "Accept Merchant", BasicOrganizations.Merchant_ID)]
        public JsonResult Accept(Guid mid)
        {
            FullResponse result = _merchantService.Accept(mid);
            return Json(result);
        }

        [HttpPost]
        [Route("Merchant/Register/AddBankAccount")]
        [Route("Merchant/AddBankAccount")]
        public JsonResult AddBankAccount(MerchantRegisterModel_Bank model)
        {
            if (ModelState.IsValid)
            {
                return Json(_merchantService.AddBankAccount(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/AddChannel")]
        [Route("Merchant/AddChannel")]
        public JsonResult AddChannel(MerchantRegisterModel_Channel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_merchantService.AddChannel(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/AddOutlet")]
        [Route("Merchant/AddOutlet")]
        public JsonResult AddOutlet(MerchantOutletViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainModel = _mapper.Map<MerchantRegisterModel_Outlet>(model);
                return Json(_merchantService.AddOutlet(domainModel));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/AddOwner")]
        [Route("Merchant/AddOwner")]
        public JsonResult AddOwner(MerchantOwnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainModel = _mapper.Map<MerchantRegisterModel_Owner>(model);
                return Json(_merchantService.AddOwner(domainModel));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/Approve")]
        [Route("Merchant/Approve")]
        [Permission("B308146A-0288-49A1-BAA1-B5BEA4A08895", "Approve Merchant",BasicOrganizations.Merchant_ID)]
        public JsonResult Approve(Guid mid)
        {
            FullResponse result = _merchantService.Approve(mid);
            return Json(result);
        }

        [HttpPost]
        [Route("Merchant/Register/ChangeMerchantState")]
        [Route("Merchant/ChangeMerchantState")]
        public JsonResult ChangeMerchantState(Guid merchantId, int state)
        {
            if (Enum.TryParse(typeof(MerchantRegisterState), state.ToString(), out object merchantState))
            {
                var inputState = (MerchantRegisterState)merchantState;
                SimpleResponse result = _merchantService.UpdateMerchantState(merchantId, inputState);
                return Json(result);
            }
            else
            {
                return Json(new SimpleResponse
                {
                    IsSuccessful = false,
                    Message = "Input MerchantState value is not valid"
                });
            }
        }

        [HttpPost]
        [Route("Merchant/Register/Complete")]
        [Route("Merchant/Complete")]
        [Permission("E8EE591A-1068-4546-9F59-9267E6C0A46D", "Complete Merchant", BasicOrganizations.Merchant_ID)]
        public JsonResult Complete(Guid mid)
        {
            FullResponse result = _merchantService.Complete(mid);
            return Json(result);
        }

        public JsonResult GetBankAccount(Guid id)
        {
            MerchantRegisterModel_Bank bankAccount = _merchantService.GetBankAccount(id);
            return Json(bankAccount);
        }

        [HttpPost]
        [Route("Merchant/Register/GetOutlet")]
        [Route("Merchant/GetOutlet")]
        public async Task<IActionResult> GetOutlet(Guid? id)
        {
            MerchantOutletViewModel model = new();
            if (id.HasValue)
            {
                var outlet = _merchantService.GetOutlet(id.Value);
                model = _mapper.Map<MerchantOutletViewModel>(outlet);
            }
            model.CountryStates = _codeListService.GetCountryStateList();
            var viewResult = await _viewRenderService.RenderToStringAsync(ControllerContext, "_Outlet", model);
            return Content(viewResult);
        }

        [HttpPost]
        [Route("Merchant/Register/GetOutlets")]
        [Route("Merchant/GetOutlets")]
        public JsonResult GetOutlets(Guid id)
        {
            List<MerchantRegisterModel_Outlet> outlets = _merchantService.GetOutlets(id);
            return Json(outlets);
        }

        [HttpPost]
        [Route("Merchant/Register/GetOwner")]
        [Route("Merchant/GetOwner")]
        public async Task<IActionResult> Getowner(Guid? id)
        {
            MerchantOwnerViewModel model = new();
            if (id.HasValue)
            {
                var owner = _merchantService.GetOwner(id.Value);
                model = _mapper.Map<MerchantOwnerViewModel>(owner);
            }
            model.DesignationList = _codeListService.GetOccupationList();
            var viewResult = await _viewRenderService.RenderToStringAsync(ControllerContext, "_Owner", model);
            return Content(viewResult);
        }

        [HttpPost]
        [Route("Merchant/Register/GetOwners")]
        [Route("Merchant/GetOwners")]
        public JsonResult GetOwners(Guid id)
        {
            List<MerchantRegisterModel_Owner> owners = _merchantService.GetOwners(id);
            return Json(owners);
        }

        [HttpPost]
        public JsonResult MerchantInbox()
        {
            List<MerchantListModel> model = _merchantService.RetrieveForInbox();
            return Json(model);
        }

        [HttpPost]
        public JsonResult MerchantList(DtReceive dtReceive)
        {
            DtReturn<MerchantListModel> model = _merchantService.RetrieveForList(dtReceive);
            return Json(model);
        }

        [HttpPost]
        [Route("Merchant/Register/RegisterInfo")]
        [Route("Merchant/RegisterInfo")]
        public JsonResult RegisterInfo(MerchantRegisterModel_Info model)
        {
            if (ModelState.IsValid)
            {
                if (model.Mid == null)
                {
                    return Json(_merchantService.RegisterInfo(model));
                }
                else
                {
                    return Json(_merchantService.UpdateInfo(model));
                }
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/RegisterServices")]
        [Route("Merchant/RegisterServices")]
        public JsonResult RegisterServices(MerchantRegisterModel_Services model)
        {
            if (ModelState.IsValid)
            {
                return Json(_merchantService.AddServices(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [Route("Merchant/Register/Reject")]
        [Route("Merchant/Reject")]
        [Permission("9B15A31A-38C2-4600-BE5D-F684263444C8", "Reject Merchant", BasicOrganizations.Merchant_ID)]
        public JsonResult Reject(MerchantRegisterRejectModel model)
        {
            FullResponse result = _merchantService.Reject(model);
            return Json(result);
        }

        [HttpPost]
        [Route("Merchant/Register/RemoveFile")]
        [Route("Merchant/RemoveFile")]
        public JsonResult RemoveFile(string mid, string name)
        {
            FullResponse result = new FullResponse();
            try
            {
                _merchantService.RemoveMerchantFile(mid, name);

                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        [Route("Merchant/Register/RemoveOutlet")]
        [Route("Merchant/RemoveOutlet")]
        public JsonResult RemoveOutlet(Guid outletId)
        {
            FullResponse response = _merchantService.RemoveOutlet(outletId);
            return Json(response);
        }

        [HttpPost]
        [Route("Merchant/Register/RemoveOwner")]
        [Route("Merchant/RemoveOwner")]
        public JsonResult RemoveOwner(Guid ownerId)
        {
            FullResponse response = _merchantService.RemoveOwner(ownerId);
            return Json(response);
        }

        [HttpPost]
        [Route("Merchant/Register/UploadFile")]
        [Route("Merchant/UploadFile")]
        public JsonResult UploadFile(MerchantUploadFileModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_merchantService.AddMerchantFile(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public JsonResult UserRegister(MerchantUserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var domainModel = _mapper.Map<UserRegisterModel>(model);
                FullResponse result = _merchantService.CreateUser(domainModel);
                return Json(result);
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }
        #endregion API

        #region Private

        private MerchantRegisterViewModel FillUpLists(MerchantRegisterViewModel model)
        {
            model.Countries = _codeListService.GetCountryList();
            model.DesignationList = _codeListService.GetOccupationList();
            model.BankNameList = _codeListService.GetBankList();
            model.CountryStates = _codeListService.GetCountryStateList();
            model.SalesPeople = _merchantService.GetSalesPersonList(model?.Info?.SalesPersonId);
            return model;
        }

        #endregion Private
    }
}
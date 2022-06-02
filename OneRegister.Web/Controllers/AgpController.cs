using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Domain.Model.AgropreneurRegistration;
using OneRegister.Domain.Model.Shared;
using OneRegister.Domain.Services.AgropreneurRegistration;
using OneRegister.Domain.Services.Shared;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "DE21E330-AEDD-4C22-BFBB-159553F777C4", Name: "Agropreneur", OrganizationId: BasicOrganizations.Agropreneur_ID, Order = 4, FasIcon = "fa-tractor")]
    public class AgpController : Controller
    {
        private readonly AgropreneurService _agpService;
        private readonly CodeListService _cL_Business;

        public AgpController(AgropreneurService agpService, CodeListService cL_Business)
        {
            _agpService = agpService;
            _cL_Business = cL_Business;
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            AGPRegisterModel model = _agpService.GetAgroById(id);
            if (model == null)
            {
                return new NotFoundResult();
            }
            return View(FillUpLists(model));
        }

        [Menu(Id: "B69C0C2F-1D2E-47FD-BE5A-4836166AECB1", Name: "Import", OrganizationId: BasicOrganizations.Agropreneur_ID, Order = 3, Parent = "DE21E330-AEDD-4C22-BFBB-159553F777C4")]
        public IActionResult Import()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "5CE1865F-6B44-46EA-AD88-312E35C12181", Name: "List", OrganizationId: BasicOrganizations.Agropreneur_ID, Order = 1, Parent = "DE21E330-AEDD-4C22-BFBB-159553F777C4")]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "78ACC2D0-7A28-4677-AC08-F82AF0F654D4", Name: "Register", OrganizationId: BasicOrganizations.Agropreneur_ID, Order = 2, Parent = "DE21E330-AEDD-4C22-BFBB-159553F777C4")]
        public ActionResult Register()
        {
            var model = FillUpLists(new AGPRegisterModel());
            return View(model);
        }

        private AGPRegisterModel FillUpLists(AGPRegisterModel m)
        {
            m.DesignationList = _cL_Business.GetOccupationList();
            m.IndustryList = _cL_Business.GetIndustryList();
            m.PuproseOfTransactionList = _cL_Business.GetRemitPurposeList();
            m.NatureOfBusinessList = _cL_Business.GetIndustryList();
            m.BankNameList = _cL_Business.GetBankList();
            m.NationalityList = _cL_Business.GetCountryList();
            m.IdentityTypeList = _cL_Business.GetIdentityTypeList();
            return m;
        }

        #region APIs

        [HttpPost]
        public JsonResult AgropreneurList(DtReceive dtReceive)
        {
            DtReturn<AGPListModel> model = _agpService.RetrieveForList(dtReceive);
            return Json(model);
        }

        [HttpPost]
        public JsonResult Edit(AGPRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_agpService.Update(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpGet]
        public IActionResult GetThumbnail(Guid agroId)
        {
            byte[] photo = _agpService.GetThumbnail(agroId);
            return File(photo, "image/jpeg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportFile(ImportFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<AGPImportModel> records = _agpService.ReadFile(model.File);
                    foreach (var record in records)
                    {
                        ModelState.Clear();
                        if (TryValidateModel(record))
                        {
                            record.IsAcceptable = true;
                        }
                        else
                        {
                            record.IsAcceptable = false;
                            record.Description = ModelState.ConcatToString();
                        }
                    }

                    var count = _agpService.ImportAcceptedRecords(records.Where(s => s.IsAcceptable == true).ToList());
                    return Json(AGPImportPreviewModel.Success($"{count} records was imported successfully"));
                }
                else
                {
                    return Json(AGPImportPreviewModel.Failure(ModelState.ConcatToString()));
                }
            }
            catch (Exception ex)
            {
                return Json(AGPImportPreviewModel.Failure(ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PreviewFile(ImportFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<AGPImportModel> records = _agpService.ReadFile(model.File);
                    foreach (var record in records)
                    {
                        ModelState.Clear();
                        if (TryValidateModel(record))
                        {
                            record.IsAcceptable = true;
                        }
                        else
                        {
                            record.IsAcceptable = false;
                            record.Description = ModelState.ConcatToString();
                        }
                    }
                    return Json(AGPImportPreviewModel.Success(records));
                }
                else
                {
                    return Json(AGPImportPreviewModel.Failure(ModelState.ConcatToString()));
                }
            }
            catch (Exception ex)
            {
                return Json(AGPImportPreviewModel.Failure(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult Register(AGPRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_agpService.Register(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        #endregion APIs
    }
}
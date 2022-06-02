using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Domain.Services.MerchantRegistration;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.MerchantRegistration;
using System;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "323DC892-64D6-4E85-8BC9-BAE0C335F215", Name: "SalesPerson", OrganizationId: BasicOrganizations.Merchant_ID, Order = 8, FasIcon = "fa-user-tie")]
    public class SalesPersonController : Controller
    {
        private readonly MerchantService _service;

        public SalesPersonController(MerchantService service)
        {
            _service = service;
        }

        #region Entries
        [HttpGet]
        [Menu("A2E5833C-8A10-49EC-857F-9CDDDEF16240", "Register Merchant Service", BasicOrganizations.Merchant_ID, Order = 2, Parent = "323DC892-64D6-4E85-8BC9-BAE0C335F215")]
        [Permission("FD584BD7-43EB-4956-9D1D-EAEEEACFBD69", "Merchant Service Registration", BasicOrganizations.Merchant_ID)]
        public IActionResult Register()
        {
            var model = new SalesPersonFormRegistrationViewModel
            {
                MerchantAccounts = _service.GetMerchantAccountList()
            };
            return View(model);
        }
        #endregion



        #region APIs
        [HttpPost]
        public JsonResult Register(SalesPersonFormRegistrationModel model)
        {
            try
            {
                FullResponse result = _service.RegisterMerchantFormBySalesPerson(model);
                return Json(result);
            }
            catch (Exception ex)
            {

                return Json(ex.ToFullResponse());
            }
        }
        [HttpPost]
        public JsonResult GetBusinessName(string businessNo)
        {
            try
            {
                FullResponse result = _service.GetBusinessName(businessNo);
                return Json(result);
            }
            catch (Exception ex)
            {

                return Json(ex.ToFullResponse());
            }
        }
        #endregion
    }
}

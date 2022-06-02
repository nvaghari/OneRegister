using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Services.Shared;
using OneRegister.Security.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers;

[Menu(Id: "30EC2EE1-B3BE-457D-BFB0-B426F02C9224", Name: "Organization", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 7, FasIcon = "fa-sitemap")]
public class OrganizationController : Controller
{
    private readonly OrganizationService _organizationService;

    public OrganizationController(OrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    [Menu(Id: "F52F9B3E-059F-4D16-8BAD-7D359B8BE045", Name: "Organization List", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 1, Parent = "30EC2EE1-B3BE-457D-BFB0-B426F02C9224")]
    [Permission(Id: "424968F9-9037-4063-BE5A-769F095363D2", Name: "View Organization Hierarchy Page", OrganizationId: BasicOrganizations.OneRegister_ID)]
    public IActionResult Index()
    {
        return View();
    }


    #region API
    [HttpPost]
    [Permission(Id: "BCB996F3-F38C-4F3D-9D17-19529B461A92", Name: "Update Organization Paths", OrganizationId: BasicOrganizations.OneRegister_ID)]
    public JsonResult UpdatePaths()
    {
        try
        {
            _organizationService.PathingOrganizations();
            return Json(SimpleResponse.Success());
        }
        catch (System.Exception ex)
        {
            return Json(SimpleResponse.FailBecause(ex.Message));
        }
    }
    [HttpPost]
    public JsonResult GetTree()
    {
        var model = _organizationService.GetOrgTree();
        return Json(model);
    }
    #endregion
}

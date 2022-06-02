using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.Audit;
using OneRegister.Web.Services.Audit;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly AuditService _auditService;

        public AuditController(AuditService auditService)
        {
            _auditService = auditService;
        }
        [HttpGet]
        [Menu(Id: "84F1ACAF-C6DA-41D0-A464-2ABFD9891E27", Name: "Log", OrganizationId: BasicOrganizations.OneRegister_ID, Order = 3, Parent = "3EBBAB5F-60AB-4EE2-8246-966CE3B781B9")]
        public IActionResult Index()
        {
            var model = new LogViewModel
            {
                FileList = _auditService.GetExistLogFileListItems()
            };
            return View(model);
        }
        [HttpPost]
        public JsonResult GetLog(string logName)
        {

            var model = _auditService.GetLogFile(logName);
            return Json(model);
        }

    }
}

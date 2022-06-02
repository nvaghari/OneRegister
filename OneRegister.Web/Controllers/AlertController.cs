using Microsoft.AspNetCore.Mvc;

namespace OneRegister.Web.Controllers
{
    public class AlertController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}

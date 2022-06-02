using Microsoft.AspNetCore.Mvc;
using OneRegister.Web.Models.Home;

namespace OneRegister.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Result(bool s)
        {
            var model = new ResultViewModel
            {
                IsSuccess = s,
                Action = TempData["action"]?.ToString(),
                Description = TempData["description"]?.ToString()
            };
            return View(model);
        }
    }
}

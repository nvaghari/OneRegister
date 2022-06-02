using Microsoft.AspNetCore.Mvc;

namespace OneRegister.Web.ViewComponents
{
    public class MainModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

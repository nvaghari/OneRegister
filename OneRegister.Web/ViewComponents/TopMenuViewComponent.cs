using Microsoft.AspNetCore.Mvc;

namespace OneRegister.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

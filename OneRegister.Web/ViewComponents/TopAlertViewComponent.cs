using Microsoft.AspNetCore.Mvc;

namespace OneRegister.Web.ViewComponents;

public class TopAlertViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}

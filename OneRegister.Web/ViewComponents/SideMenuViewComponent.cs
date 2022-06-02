using Microsoft.AspNetCore.Mvc;
using OneRegister.Web.Services.Menu.SideMenu;
using System.Linq;

namespace OneRegister.Web.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {
        private readonly ISideMenuService _sideMenuService;

        public SideMenuViewComponent(ISideMenuService sideMenuService)
        {
            _sideMenuService = sideMenuService;
        }
        public IViewComponentResult Invoke()
        {
            var menus = _sideMenuService.GetAuthorizedMenus(UserClaimsPrincipal);
            var titleMenus = menus.Where(m => m.Parent is null).OrderBy(m => m.Order).ToList();
            string version = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var model = new SideMenuComponentModel()
            {
                AppVersion = version,
                Menus = menus,
                TitleMenus = titleMenus
            };
            return View(model);
        }
    }
}

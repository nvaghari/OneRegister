using System.Collections.Generic;
using System.Security.Claims;

namespace OneRegister.Web.Services.Menu.SideMenu
{
    public interface ISideMenuService
    {
        List<SideMenuModel> GetCollectedMenus();
        List<SideMenuModel> GetAuthorizedMenus(ClaimsPrincipal User);
    }
}
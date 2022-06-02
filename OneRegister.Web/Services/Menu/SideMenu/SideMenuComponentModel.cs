using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Web.Services.Menu.SideMenu
{
    public class SideMenuComponentModel
    {
        public List<SideMenuModel> Menus { get; set; } = new();
        public List<SideMenuModel> TitleMenus { get; set; } = new();
        public string AppVersion { get; set; } = string.Empty;


        public List<SideMenuModel> GetSubMenus(SideMenuModel parent)
        {
            return Menus.Where(m => m.Parent == parent.Id).OrderBy(m => m.Order).ToList();
        }
    }
}

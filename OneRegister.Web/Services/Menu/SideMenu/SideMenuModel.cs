using OneRegister.Domain.Extentions;
using System;

namespace OneRegister.Web.Services.Menu.SideMenu
{
    public class SideMenuModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FasIcon { get; set; }
        public int Order { get; set; }
        public Guid? Parent { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public string Controller => ClassName?.Replace("Controller", string.Empty);
        public string SanitizedName => Name?.SanitizeForJavaScript();

        public string GetIcon() => FasIcon ?? "fa-question";
    }
}
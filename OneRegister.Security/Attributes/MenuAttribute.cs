using System;

namespace OneRegister.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class MenuAttribute : Attribute, IPermissionAttribute
    {
        public MenuAttribute(string Id, string Name, string OrganizationId)
        {
            this.Id = Id;
            this.Name = Name;
            this.OrganizationId = OrganizationId;
        }
        public int Order { get; set; } = 0;
        public string Id { get; private set; }
        public string Name { get;private set; }
        public string OrganizationId { get; private set; }
        public string Parent { get; set; } = string.Empty;
        public string FasIcon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }
}

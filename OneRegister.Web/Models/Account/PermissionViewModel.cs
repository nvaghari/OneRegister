using System;

namespace OneRegister.Web.Models.Account
{
    public class PermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string AttributeType { get; set; }
        public string OrganizationName { get; set; }
    }
}

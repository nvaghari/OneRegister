

using System;

namespace OneRegister.Domain.Model.Account
{
    public class PermissionListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string AttributeType { get; set; }
        public string OrganizationName { get; set; }
    }
}

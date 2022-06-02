using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class InlinePermissionAttribute : Attribute, IPermissionAttribute
    {
        public InlinePermissionAttribute(string Id, string Name, string OrganizationId)
        {
            this.Id = Id;
            this.Name = Name;
            this.OrganizationId = OrganizationId;
        }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string OrganizationId { get; private set; }
    }
}

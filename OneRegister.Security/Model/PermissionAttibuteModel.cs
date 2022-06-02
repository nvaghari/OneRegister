using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Model
{
    public class PermissionAttibuteModel : IEquatable<PermissionAttibuteModel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrganizationId { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string DomainName { get; set; }
        public string AttributeType { get; set; }

        public Guid OrganizationGuid => string.IsNullOrEmpty(OrganizationId) ? Guid.Empty : Guid.Parse(OrganizationId);
        public Guid Guid => string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
        public override bool Equals(object obj) => Equals(obj as PermissionAttibuteModel);
        public bool Equals(PermissionAttibuteModel other)
        {
            return other.Name == Name
                && string.Equals(other.OrganizationId, OrganizationId, StringComparison.OrdinalIgnoreCase)
                && other.ClassName == ClassName
                && other.MethodName == MethodName;
        }
        public override int GetHashCode()
        {
            return (Name,OrganizationId,ClassName,MethodName).GetHashCode();
        }

        public static bool operator ==(PermissionAttibuteModel a, PermissionAttibuteModel b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(PermissionAttibuteModel a, PermissionAttibuteModel b) => !(a == b);

    }
}

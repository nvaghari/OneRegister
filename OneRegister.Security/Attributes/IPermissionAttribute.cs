using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Security.Attributes
{
    public interface IPermissionAttribute
    {
        string Id { get; }
        string Name { get; }
        string OrganizationId { get; }
    }
}

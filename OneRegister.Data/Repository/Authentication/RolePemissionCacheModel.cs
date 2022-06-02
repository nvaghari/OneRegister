using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Repository.Authentication
{
    public class RolePemissionCacheModel
    {
        public Guid PermissionId { get; set; }
        public string AttributeType { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}

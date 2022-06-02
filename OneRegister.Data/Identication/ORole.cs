using Microsoft.AspNetCore.Identity;
using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;

namespace OneRegister.Data.Identication
{
    public class ORole : IdentityRole<Guid>, IBaseEntity,IOrganizedEntity
    {
        public ORole()
        {
            CreatedAt = ModifiedAt = DateTime.Now;
        }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }
        public StateOfEntity State { get; set; }

        public bool IsSystemic { get; set; }
        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}

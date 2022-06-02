using Microsoft.AspNetCore.Identity;
using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Identication
{
    public class OUser : IdentityUser<Guid>, IBaseEntity,IOrganizedEntity
    {
        public OUser()
        {
            CreatedAt = ModifiedAt = DateTime.Now;
        }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public Guid? ModifiedBy { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string Name { get; set; }
        public bool IsSystemic { get; set; }
        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public StateOfEntity State { get; set; }
    }
}

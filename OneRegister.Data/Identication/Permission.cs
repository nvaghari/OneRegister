using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Identication
{
    [Table("Permissions", Schema = SchemaNames.Security)]
    public class Permission : BaseEntity,IOrganizedEntity
    {
        [StringLength(DataLength.LONGNAME)]
        public string MethodName { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string ClassName { get; set; }

        [StringLength(DataLength.Name)]
        public string AttributeType { get; set; }

        //navigation property
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ORole> Roles { get; set; }
    }
}

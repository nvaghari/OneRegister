using OneRegister.Data.Contract;
using OneRegister.Data.Entities.Application;
using OneRegister.Data.Identication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.SuperEntities
{
    public enum OrganizationType
    {
        Root = 0,
        School = 1,
        Merchant = 2,
        Agropreneur = 3
    }
    [Table(nameof(Organization), Schema = SchemaNames.Base)]
    public class Organization : BaseEntity, IOrganizationEntity
    {
        public int Sequencer { get; set; }
        [StringLength(DataLength.Description)]
        public string Path { get; set; }
        public bool IsSystemic { get; set; }
        public ICollection<Member> Members { get; set; }
        public ICollection<OrganizationFile> OrganizationFiles { get; set; }
        public Organization Parent { get; set; }
        public Guid? ParentId { get; set; }
        public ICollection<Site> Sites { get; set; }
        public ICollection<OUser> Users { get; set; }
        public ICollection<ORole> Roles { get; set; }
        public ICollection<Permission> Claims { get; set; }
        public ICollection<Setting> Settings { get; set; }
    }
}
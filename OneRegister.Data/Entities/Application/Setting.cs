using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.Application
{
    public enum SettingType
    {
        Other,
        Email
    }
    [Table(nameof(Setting), Schema = SchemaNames.Application)]
    public class Setting : BaseEntity,IOrganizedEntity
    {
        public SettingType SettingType { get; set; }
        [StringLength(DataLength.Remark)]
        public string Value { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

    }
}

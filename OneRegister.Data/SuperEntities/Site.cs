using OneRegister.Data.Contract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OneRegister.Data.Contract.Constants;
namespace OneRegister.Data.SuperEntities
{
    public enum SiteType
    {
        Root = 0,
        Class = 1,
        HomeRoom = 2,
        Outlet = 3
    }
    [Table(nameof(Site), Schema = SchemaNames.Base)]
    public class Site : BaseEntity,IOrganizedEntity
    {
        [StringLength(DataLength.ADDRESS)]
        public string Address { get; set; }

        [StringLength(DataLength.PHONE)]
        public string Fax { get; set; }

        [Column(TypeName = "decimal(8,6)")]
        public decimal? Lat { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Lng { get; set; }

        [StringLength(DataLength.ADDRESS)]
        public string OperatingDaysHours { get; set; }

        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
        [StringLength(DataLength.IDENTITY)]
        public string PostCode { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string RegionState { get; set; }

        [StringLength(DataLength.Remark)]
        public string Remark { get; set; }

        [StringLength(DataLength.PHONE)]
        public string Tel { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string Town { get; set; }

        public int? Year { get; set; }
    }
}

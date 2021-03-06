// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OneRegister.Data.Entities.MasterCardGems
{
    /// <summary>
    /// Codelist of possible address types, eg: home, work, post, billing, shipping
    /// </summary>
    [Table("CL_AddressType", Schema = "Entity")]
    public partial class ClAddressType
    {
        [Key]
        [Column("_Code")]
        [StringLength(32)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }
        [Column("xIsDeleted")]
        public int XIsDeleted { get; set; }
    }
}
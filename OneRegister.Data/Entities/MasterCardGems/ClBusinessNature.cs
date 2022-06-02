﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OneRegister.Data.Entities.MasterCardGems
{
    /// <summary>
    /// Codelist of BusinessNatures as part of Industry-BusinessNature pair
    /// </summary>
    [Table("CL_BusinessNature", Schema = "Entity")]
    public partial class ClBusinessNature
    {
        [Key]
        [Column("_Code")]
        [StringLength(32)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }
        [Required]
        [StringLength(300)]
        public string LongName { get; set; }
        /// <summary>
        /// AML/CDD Risk Level [1-5] with Lowest=1, Highest=5
        /// </summary>
        public short RiskLevel { get; set; }
        [Column("xIsDeleted")]
        public int XIsDeleted { get; set; }
    }
}
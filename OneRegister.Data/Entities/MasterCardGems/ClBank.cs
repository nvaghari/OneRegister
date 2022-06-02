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
    /// Codelist of Banks that are used for Bank Account information
    /// </summary>
    [Table("CL_Bank", Schema = "Entity")]
    public partial class ClBank
    {
        /// <summary>
        /// Country where this Bank operates. CountryCode is based on ISO-3166-2
        /// </summary>
        [Required]
        [StringLength(3)]
        public string CountryCodeNum { get; set; }
        /// <summary>
        /// Name of Bank
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BankName { get; set; }
        /// <summary>
        /// 1=BankCode, 2=RPP/SWIFT, 3=FPX
        /// </summary>
        [Key]
        public short CodeType { get; set; }
        /// <summary>
        /// Bank Code for the specific CodeType
        /// </summary>
        [Key]
        [StringLength(32)]
        public string BankCode { get; set; }
        /// <summary>
        /// Bank Account Number format in regular expression syntax
        /// </summary>
        [StringLength(100)]
        public string BankAcctNoFormat { get; set; }
        [Column("xIsDeleted")]
        public int XIsDeleted { get; set; }

        public virtual ClCountry CountryCodeNumNavigation { get; set; }
    }
}
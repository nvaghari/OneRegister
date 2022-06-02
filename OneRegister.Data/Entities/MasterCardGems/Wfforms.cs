﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OneRegister.Data.Entities.MasterCardGems
{
    [Table("WFForms", Schema = "OneReg")]
    public partial class Wfforms
    {
        public int PkId { get; set; }
        [Column("WFInstanceID")]
        public int? WfinstanceId { get; set; }
        [StringLength(50)]
        public string OrgId { get; set; }
        [Key]
        [StringLength(20)]
        public string FormNo { get; set; }
        [Required]
        [StringLength(32)]
        public string FormType { get; set; }
        public int FormVersion { get; set; }
        [StringLength(50)]
        public string FormName { get; set; }
        [StringLength(32)]
        public string FormStatus { get; set; }
        [StringLength(32)]
        public string Channel { get; set; }
        [StringLength(800)]
        public string Remarks { get; set; }
        public string FormContent { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        [StringLength(30)]
        public string MobileNo { get; set; }
        public string RawMsg { get; set; }
        /// <summary>
        /// Date this record was first created
        /// </summary>
        [Column("xDateCreated", TypeName = "datetime")]
        public DateTime? XDateCreated { get; set; }
        /// <summary>
        /// Date this record was last modified
        /// </summary>
        [Column("xDateModified", TypeName = "datetime")]
        public DateTime? XDateModified { get; set; }
        /// <summary>
        /// User who first created this record
        /// </summary>
        [Column("xCreatedBy")]
        [StringLength(100)]
        public string XCreatedBy { get; set; }
        /// <summary>
        /// User who last modified this record
        /// </summary>
        [Column("xModifiedBy")]
        [StringLength(100)]
        public string XModifiedBy { get; set; }
        /// <summary>
        /// User who has currently locked this record for editing
        /// </summary>
        [Column("xEditLockBy")]
        [StringLength(100)]
        public string XEditLockBy { get; set; }
        /// <summary>
        /// Datetime when this record was locked for editing
        /// </summary>
        [Column("xEditLockTime", TypeName = "datetime")]
        public DateTime? XEditLockTime { get; set; }
        /// <summary>
        /// Is this record marked as deleted? 0=No, 1=Yes
        /// </summary>
        [Column("xIsDeleted")]
        public int XIsDeleted { get; set; }

        [ForeignKey(nameof(Channel))]
        [InverseProperty(nameof(ClChannel.Wfforms))]
        public virtual ClChannel ChannelNavigation { get; set; }
        [ForeignKey(nameof(FormType))]
        [InverseProperty(nameof(ClFormType.Wfforms))]
        public virtual ClFormType FormTypeNavigation { get; set; }
        [ForeignKey(nameof(WfinstanceId))]
        [InverseProperty("Wfforms")]
        public virtual Wfinstance Wfinstance { get; set; }
    }
}
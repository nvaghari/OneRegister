// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneRegister.Data.Entities.MasterCardGems
{
    public partial class ErrorLog_GetResult
    {
        public int ErrTicket { get; set; }
        public int ParentID { get; set; }
        public int? ErrCodeNo { get; set; }
        public string ErrCodeStr { get; set; }
        public string ErrSource { get; set; }
        public string ErrMsg { get; set; }
        public string ErrContext { get; set; }
        public string PVModule { get; set; }
        public string PVFunction { get; set; }
        public string PVLine { get; set; }
        public DateTime xDateCreated { get; set; }
        public string xCreatedBy { get; set; }
        public string ProcessID { get; set; }
        public int? ThreadID { get; set; }
        public string UserMsg { get; set; }
    }
}

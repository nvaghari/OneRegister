﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneRegister.Data.Entities.MasterCardGems
{
    public partial class CDDActionIV_ListBankAcctInfoResult
    {
        public int CDDActionIV { get; set; }
        public string BankAcctCodeType { get; set; }
        public string BankBIC { get; set; }
        public string BankAcctNo { get; set; }
        public string ICBankAcctName { get; set; }
        public string IVBankAcctName { get; set; }
        public string IVStatus { get; set; }
        public DateTime? IVStartDate { get; set; }
    }
}
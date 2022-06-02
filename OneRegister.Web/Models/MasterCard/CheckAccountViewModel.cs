using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.MasterCard
{
    public class CheckAccountViewModel
    {
        [CustomRequired]
        public string RefId { get; set; }
        [CustomRequired]
        public string AcctNo { get; set; }
        [CustomRequired]
        public string BankCode { get; set; }
        public string AccountName { get; set; }
    }
}

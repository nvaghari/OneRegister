using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.RPPApi.Model
{
    public class BankAccountSendModel
    {
        public string RefID { get; set; }
        public string AcctNo { get; set; }
        public int SourceID { get; set; }
        public string BankCode { get; set; }
        public string ActionType { get; set; }
        public string AuthKey { get; private set; }
    }
}

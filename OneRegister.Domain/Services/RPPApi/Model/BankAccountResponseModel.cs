using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.RPPApi.Model
{
    public class BankAccountResponseModel
    {
        public bool IsSuccessful => !string.IsNullOrEmpty(Status) && Status == "0";
        public string Status { get; set; }
        public string RetMsg { get; set; }
        public string RetID { get; set; }
    }
}

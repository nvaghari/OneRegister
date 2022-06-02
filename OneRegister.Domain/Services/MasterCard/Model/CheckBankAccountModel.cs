using OneRegister.Domain.Services.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.Model
{
    public class CheckBankAccountModel
    {
        public string RefId { get; set; }
        public string AcctNo { get; set; }
        public string BankCode { get; set; }
        public int SourceId { get; set; }
        public string ActionType => "ACCTINFO";
        public string AuthKey { get; private set; }
        internal void CalculateHash(string secretKey,int sourceId)
        {
            SourceId = sourceId;
            var combinedText = secretKey + SourceId.ToString() + ActionType + RefId;

            AuthKey = CryptoService.SHA256ToHex(combinedText);
        }
    }
}

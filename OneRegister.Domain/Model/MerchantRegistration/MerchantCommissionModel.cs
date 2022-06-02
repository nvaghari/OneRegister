using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.MerchantRegistration
{
    public class MerchantCommissionModel
    {
        [JsonIgnore]
        public string MerchantName { get; set; }
        [JsonIgnore]
        public Guid Mid { get; set; }
        //Setup Cost
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Setup_OneTime { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Setup_Monthly { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Setup_Annual { get; set; }

        //EDC Terminal Cost
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EDC_Installation { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EDC_Monthly { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EDC_Deposit { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EDC_TerminalDamage { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EDC_PaperRollDamage { get; set; }

        //Terminal Type
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TerminalType_Sunmi { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TerminalType_NewlandGPRS { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TerminalType_N6 { get; set; }

        //Payment Terms

        //Credit Card
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CreditCard_Frequency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CreditCard_Currency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_MinPayout { get; set; }

        //MyDebit
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MyDebit_Frequency { get; set; }
        public string MyDebit_Currency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MyDebit_MinPayout { get; set; }

        //FPX
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FPX_Frequency { get; set; }
        public string FPX_Currency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? FPX_MinPayout { get; set; }

        //EWallets
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? EWallets_Frequency { get; set; }
        public string EWallets_Currency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_MinPayout { get; set; }

        //DuitNow QR
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DuitNowQR_Frequency { get; set; }
        public string DuitNowQR_Currency { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? DuitNowQR_MinPayout { get; set; }

        //MDR
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_Visa_POS_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_Visa_POS_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_MasterCard_POS_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_MasterCard_POS_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_Visa_EComm_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_Visa_EComm_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_MasterCard_EComm_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CreditCard_MasterCard_EComm_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MyDebit_All_POS_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MyDebit_All_POS_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? FPX_Online_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? FPX_Online_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_M1_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_M1_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_Boost_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_Boost_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_TNG_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_TNG_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_Alipay_CN_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_Alipay_CN_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_WeChat_CN_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_WeChat_CN_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_WeChat_MY_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_WeChat_MY_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_MayBank_QR_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? EWallets_MayBank_QR_M { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? DuitNowQR_DuitNowQR_P { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? DuitNowQR_DuitNowQR_M { get; set; }

        //Remark
        [JsonIgnore]
        public string Remark { get; set; }
    }
}

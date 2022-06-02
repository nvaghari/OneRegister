using System;
using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.MerchantRegistration
{
    public class MerchantCommissionViewModel
    {
        private const string CURRENCY = "RM";
        public string MerchantName { get; set; }
        public Guid Mid { get; set; }
        //Setup Cost
        [Display(Name = "Setup Costs (One Time) (MYR)")]
        public decimal? Setup_OneTime { get; set; }
        [Display(Name = "Maintenance Fee (Monthly) (MYR)")]
        public decimal? Setup_Monthly { get; set; }
        [Display(Name = "Maintenance Fee (Annual) (MYR)")]
        public decimal? Setup_Annual { get; set; }

        //EDC Terminal Cost
        [Display(Name = "Installation Fee (MYR)")]
        public decimal? EDC_Installation { get; set; }
        [Display(Name = "Monthly rental for each unit (MYR)")]
        public decimal? EDC_Monthly { get; set; }
        [Display(Name = "Deposit (MYR)",Description = "To be refunded if returned in the same condition that Merchant received it. Otherwise, the deposit will be forfeited")]
        public decimal? EDC_Deposit { get; set; }
        [Display(Name = "Penalty for loss or damaged of each EDC terminal (MYR)")]
        public decimal? EDC_TerminalDamage { get; set; }
        [Display(Name = "Paper Roll Cover Broken/ Damaged (MYR)")]
        public decimal? EDC_PaperRollDamage { get; set; }

        //Terminal Type
        [Display(Name = "Sunmi (Qty)")]
        public int? TerminalType_Sunmi { get; set; }
        [Display(Name = "Newland GPRS (Qty)")]
        public int? TerminalType_NewlandGPRS { get; set; }
        [Display(Name = "N6 (Qty)")]
        public int? TerminalType_N6 { get; set; }

        //Payment Terms

        //Credit Card
        [Display(Name = "Payment Frequency",Description ="T+ Business Days")]
        public int? CreditCard_Frequency { get; set; }
        [Display(Name = "Payment Currency")]
        public string CreditCard_Currency { get; set; }
        [Display(Name = "Minimum Payout")]
        public decimal? CreditCard_MinPayout { get; set; }

        //MyDebit
        [Display(Name = "Payment Frequency", Description = "T+ Business Days")]
        public int? MyDebit_Frequency { get; set; }
        [Display(Name = "Payment Currency")]
        public string MyDebit_Currency { get; set; }
        [Display(Name = "Minimum Payout")]
        public decimal? MyDebit_MinPayout { get; set; }

        //FPX
        [Display(Name = "Payment Frequency", Description = "T+ Business Days")]
        public int? FPX_Frequency { get; set; }
        [Display(Name = "Payment Currency")]
        public string FPX_Currency { get; set; }
        [Display(Name = "Minimum Payout")]
        public decimal? FPX_MinPayout { get; set; }

        //EWallets
        [Display(Name = "Payment Frequency", Description = "T+ Business Days")]
        public int? EWallets_Frequency { get; set; }
        [Display(Name = "Payment Currency")]
        public string EWallets_Currency { get; set; }
        [Display(Name = "Minimum Payout")]
        public decimal? EWallets_MinPayout { get; set; }

        //DuitNow QR
        [Display(Name = "Payment Frequency", Description = "T+ Business Days")]
        public int? DuitNowQR_Frequency { get; set; }
        [Display(Name = "Payment Currency")]
        public string DuitNowQR_Currency { get; set; }
        [Display(Name = "Minimum Payout")]
        public decimal? DuitNowQR_MinPayout { get; set; }

        //MDR
        [Display(Name = "VISA (POS)",Description ="%")]
        public decimal? CreditCard_Visa_POS_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? CreditCard_Visa_POS_M { get; set; }
        [Display(Name = "MasterCard (POS)",Description ="%")]
        public decimal? CreditCard_MasterCard_POS_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? CreditCard_MasterCard_POS_M { get; set; }
        [Display(Name = "VISA (eComm)", Description = "%")]
        public decimal? CreditCard_Visa_EComm_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? CreditCard_Visa_EComm_M { get; set; }
        [Display(Name = "MasterCard (eComm)", Description = "%")]
        public decimal? CreditCard_MasterCard_EComm_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? CreditCard_MasterCard_EComm_M { get; set; }
        [Display(Name = "All Participating Banks (POS)", Description = "%")]
        public decimal? MyDebit_All_POS_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? MyDebit_All_POS_M { get; set; }
        [Display(Name = "OnlineBanking", Description = "%")]
        public decimal? FPX_Online_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? FPX_Online_M { get; set; }
        [Display(Name = "MobilityOne eM-ONEi", Description = "%")]
        public decimal? EWallets_M1_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_M1_M { get; set; }
        [Display(Name = "Boost", Description = "%")]
        public decimal? EWallets_Boost_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_Boost_M { get; set; }
        [Display(Name = "Touch 'n Go eWallet", Description = "%")]
        public decimal? EWallets_TNG_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_TNG_M { get; set; }
        [Display(Name = "Alipay (China)", Description = "%")]
        public decimal? EWallets_Alipay_CN_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_Alipay_CN_M { get; set; }
        [Display(Name = "WeChat Pay (China)", Description = "%")]
        public decimal? EWallets_WeChat_CN_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_WeChat_CN_M { get; set; }
        [Display(Name = "WeChat Pay (Malaysia)", Description = "%")]
        public decimal? EWallets_WeChat_MY_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_WeChat_MY_M { get; set; }
        [Display(Name = "Maybank QRPay", Description = "%")]
        public decimal? EWallets_MayBank_QR_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? EWallets_MayBank_QR_M { get; set; }
        [Display(Name = "DuitNow QR", Description = "%")]
        public decimal? DuitNowQR_DuitNowQR_P { get; set; }
        [Display(Name = "", Description = "RM/Txn")]
        public decimal? DuitNowQR_DuitNowQR_M { get; set; }

        //Remark
        public string Remark { get; set; }

        //Helpers
        public string Display(decimal? amount)
        {
            if (!amount.HasValue) return string.Empty;

            return CURRENCY+amount.Value.ToString("0.00");
        }
        public string Display(int? amount)
        {
            if (!amount.HasValue) return string.Empty;

            return amount.ToString();
        }
        public string Display(string text)
        {
            return text;
        }
        public string Display(decimal? p,decimal? m)
        {
            if (!p.HasValue && !m.HasValue) return string.Empty;
            if (p.HasValue && m.HasValue) return "error!";
            decimal amount;
            string prefix;
            if (p.HasValue)
            {
                amount = p.Value;
                prefix = "%";
            }
            else
            {
                amount = m.Value;
                prefix = CURRENCY;
            }

            return prefix + amount.ToString("0.00");
        }
    }
}

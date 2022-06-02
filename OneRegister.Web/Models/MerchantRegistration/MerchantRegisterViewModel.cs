using OneRegister.Domain.Model.MerchantRegistration;
using System.Collections.Generic;
using OneRegister.Data.Entities.MerchantRegistration;

namespace OneRegister.Web.Models.MerchantRegistration;

public class MerchantRegisterViewModel : MerchantRegisterModel
{
    public Dictionary<string, string> Countries { get; set; }
    public Dictionary<string, string> CountryStates { get; set; }
    public Dictionary<string, string> DesignationList { get; set; }
    public Dictionary<string, string> BankNameList { get; set; }
    public Dictionary<string, string> SalesPeople { get; set; }

    public bool IsEditable => MerchantStatus == MerchantStatus.Incomplete || MerchantStatus == MerchantStatus.Inadequate;
}

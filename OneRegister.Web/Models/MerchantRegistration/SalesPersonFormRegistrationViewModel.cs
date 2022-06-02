using OneRegister.Domain.Model.MerchantRegistration;
using System.Collections.Generic;

namespace OneRegister.Web.Models.MerchantRegistration
{
    public class SalesPersonFormRegistrationViewModel : SalesPersonFormRegistrationModel
    {
        public Dictionary<string,string> MerchantAccounts { get; set; } = new Dictionary<string,string>();
    }
}

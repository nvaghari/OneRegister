using OneRegister.Domain.Validation.Attributes;

namespace OneRegister.Web.Models.MerchantRegistration
{
    public class SalesPersonViewModel
    {
        [CustomRequired]
        public string Name { get; set; }
        [Email]
        public string Email { get; set; }
    }
}

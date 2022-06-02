using OneRegister.Domain.Validation.Attributes;

namespace OneRegister.Web.Models.MerchantRegistration
{
    public class MerchantUserRegisterViewModel
    {
        [CustomRequired]
        [Email]
        public string Email { get; set; }

        [CustomRequired]
        public string Name { get; set; }

        [CustomRequired]
        public string UserName { get; set; }

        [CustomRequired]
        public string Password { get; set; }

        [CustomRequired]
        public string PasswordConfirm { get; set; }

        [Number]
        [CustomRequired]
        public string Phone { get; set; }
    }
}

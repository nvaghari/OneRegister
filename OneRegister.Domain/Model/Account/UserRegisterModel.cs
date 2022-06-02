using System;

namespace OneRegister.Domain.Model.Account
{
    public class UserRegisterModel
    {
        public Guid OrganizationId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Phone { get; set; }
    }
}

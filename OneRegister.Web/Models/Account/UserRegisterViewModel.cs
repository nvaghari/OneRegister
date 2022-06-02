using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;

namespace OneRegister.Web.Models.Account
{
    public class UserRegisterViewModel
    {
        public Dictionary<string, string> Organizations { get; set; }

        [CustomRequired]
        public Guid OrganizationId { get; set; }

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
        public string Phone { get; set; }
    }
}

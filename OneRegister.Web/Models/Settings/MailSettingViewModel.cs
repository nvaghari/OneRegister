using OneRegister.Domain.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.Settings
{
    public class MailSettingViewModel
    {
        [CustomRequired]
        public string SmtpHost { get; set; }

        [CustomRequired]
        public int SmtpPort { get; set; }

        [CustomRequired]
        public string User { get; set; }

        [CustomRequired]
        public string Password { get; set; }

        [CustomRequired]
        [Email]
        public string Email { get; set; }

        [Email]
        public string EmailTo { get; set; }

        public bool UseSecureConnection { get; set; }
        public bool UseAuthentication { get; set; }
        public string TestText { get; set; }
    }
}

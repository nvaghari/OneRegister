using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.Settings
{
    public class MailSettingModel
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool UseSecureConnection { get; set; }
        public bool UseAuthentication { get; set; }
    }
}

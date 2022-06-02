using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.Notification
{
    public class SendEmailModel
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool UseSecureConnection { get; set; }
        public bool UseAuthentication { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
    }
}

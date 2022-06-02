using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.Notification;
using OneRegister.Domain.Model.Notification;
using OneRegister.Domain.Services.NotificationFactory;
using OneRegister.Domain.Services.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.Email
{
    public class EmailService
    {
        private readonly SettingService _settingService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(SettingService settingService,ILogger<EmailService> logger)
        {
            _settingService = settingService;
            _logger = logger;
        }
        public void Send(SendEmailModel model)
        {
            var client = new SmtpClient(model.SmtpHost, model.SmtpPort)
            {
                Credentials = new NetworkCredential(model.User, model.Password)
            };
            var message = new MailMessage
            {
                From = new MailAddress(model.From, "Merchant Support Email"),
                Subject = model.Subject,
                IsBodyHtml = model.IsHtml,
                Body = model.Body
            };
            message.To.Add(new MailAddress(model.To, "Nader Vaghari"));

            client.Send(message);
        }
        public void Send(NotificationTask notification)
        {
            CheckEmailAddress(notification.To);
            var emailSetting = _settingService.GetEmail();
            var client = new SmtpClient(emailSetting.SmtpHost, emailSetting.SmtpPort)
            {
                Credentials = new NetworkCredential(emailSetting.User, emailSetting.Password)
            };
            var message = new MailMessage
            {
                From = new MailAddress(emailSetting.Email, "Merchant Support Email"),
                Subject = notification.Subject,
                IsBodyHtml = false,
                Body = notification.Message
            };
            message.To.Add(new MailAddress(notification.To, notification.Name));
            client.Send(message);
            _logger.LogWarning($"Email was sent to {notification.Name} ({notification.To})");
        }

        private static void CheckEmailAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ApplicationException("email address is null or empty");
            }
            if (!Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",RegexOptions.IgnoreCase,TimeSpan.FromMilliseconds(300)))
            {
                throw new ApplicationException("email address is not in correct format");
            }
        }

        //private void AfterSendingEmail(object sender, AsyncCompletedEventArgs e)
        //{
        //    if(e.UserState is NotificationTask)
        //    {
        //        var task = e.UserState as NotificationTask;
        //        if (e.Cancelled)
        //        {
        //            _logger.LogError($"sending email for task id {task.Id} was canceled");
        //            //_notificationService.TaskFail(task.Id, "Canceled");
        //        }else if(e.Error != null)
        //        {
        //            _logger.LogError($"sending email for task id {task.Id} was failed because {e.Error}");
        //            //_notificationService.TaskFail(task.Id, e.Error.ToString());
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Email was sent successfully");
        //            //_notificationService.TaskDone(task.Id);
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogError("sender object isn't NotificationTask");
        //    }
        //}
    }
}

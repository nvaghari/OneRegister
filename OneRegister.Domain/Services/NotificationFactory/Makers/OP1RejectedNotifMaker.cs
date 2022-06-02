using Microsoft.Extensions.Logging;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Data.Entities.Notification;
using OneRegister.Data.Identication;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.MerchantRegistration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneRegister.Domain.Services.NotificationFactory.Makers
{
    public class OP1RejectedNotifMaker : INotificationMaker
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<OP1RejectedNotifMaker> _logger;
        private readonly MerchantService _merchantService;
        private readonly UserService _userService;

        public OP1RejectedNotifMaker(
            NotificationService notificationService,
            ILogger<OP1RejectedNotifMaker> logger,
            MerchantService merchantService,
            UserService userService
            )
        {
            this._notificationService = notificationService;
            this._logger = logger;
            this._merchantService = merchantService;
            this._userService = userService;
        }
        public bool IsEligible(ActionType actionType)
        {
            return actionType == ActionType.OP1Rejected;
        }

        public void Make(NotificationJob notificationJob)
        {
            try
            {
                var merchant = _merchantService.GetAsAdmin(notificationJob.RefId.Value, false, m => m.MerchantInfo);
                List<OUser> users = new();

                OUser merchantUser = _userService.GetAsAdmin(merchant.CreatedBy.Value, asNoTracking: true);
                users.Add(merchantUser);

                OUser salesPerson = _userService.GetAsAdmin(merchant.MerchantInfo.SalesPersonId.Value, asNoTracking: true);
                users.Add(salesPerson);

                List<NotificationTask> tasks = new();
                foreach (var user in users)
                {
                    if (user.State != StateOfEntity.Complete) continue;
                    tasks.Add(new NotificationTask
                    {
                        Name = user.Name,
                        NotificationJobId = notificationJob.Id,
                        NotificationType = NotificationType.Email,
                        State = StateOfEntity.InProgress,
                        To = user.Email,
                        Subject = $"OneRegister Merchant Notification: {merchant.MerchantInfo.Name} your form is not complete",
                        Message = CreateEmailMessage(merchant)
                    });
                }
                _notificationService.AddTaskRange(tasks);
                _notificationService.JobDone(notificationJob.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create task for this job id {notificationJob.Id}");
                _notificationService.JobFail(notificationJob.Id, ex.Message);
            }
        }
        private static string CreateEmailMessage(Merchant merchant)
        {
            var text = new StringBuilder();
            text.AppendLine("Dear Valued Merchant,");
            text.AppendLine("You have a message from OneRegister Merchant Portal:");
            text.AppendLine();
            text.AppendLine($"Merchant name: {merchant.MerchantInfo.Name}");
            text.AppendLine($"Action by Merchant: {merchant.MerchantInfo.RejectRemark}");
            text.AppendLine();
            text.AppendLine("Thank you.");
            text.AppendLine();
            text.AppendLine("Best Regards,");
            text.AppendLine("MobilityOne Sdn Bhd");
            return text.ToString();
        }
    }
}

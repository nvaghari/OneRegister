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
    public class CommissionProvidedNotifMaker : INotificationMaker
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<CommissionProvidedNotifMaker> _logger;
        private readonly MerchantService _merchantService;
        private readonly UserService _userService;

        public CommissionProvidedNotifMaker(
            NotificationService notificationService,
            ILogger<CommissionProvidedNotifMaker> logger,
            MerchantService merchantService,
            UserService userService)
        {
            _notificationService = notificationService;
            _logger = logger;
            _merchantService = merchantService;
            _userService = userService;
        }
        public bool IsEligible(ActionType actionType)
        {
            return actionType == ActionType.CommissionProvided;
        }

        public void Make(NotificationJob notificationJob)
        {
            try
            {
                var merchant = _merchantService.GetAsAdmin(notificationJob.RefId.Value, false, m => m.MerchantInfo);
                List<OUser> users = new();

                OUser merchantUser = _userService.GetAsAdmin(merchant.CreatedBy.Value, asNoTracking: true);
                users.Add(merchantUser);

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
                        Subject = $"OneRegister Merchant Notification: Commission has provided by salesperson ({merchant.MerchantInfo.Name})",
                        Message = CreateEmailMessage(merchant,user)
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
        private static string CreateEmailMessage(Merchant merchant, OUser user)
        {
            var text = new StringBuilder();
            text.AppendLine($"Dear {user.Name},");
            text.AppendLine("You have a message from OneRegister Merchant Portal:");
            text.AppendLine();
            text.AppendLine($"Merchant name: {merchant.MerchantInfo.Name}");
            text.AppendLine("Action by Merchant: “Commercial rate” has been updated by SalesPerson, merchant may proceed to tick <I Agree>, click button <Next> & click button <Complete> after  agrees to accept the Terms and Conditions.");
            text.AppendLine();
            text.AppendLine("Thank you.");
            text.AppendLine();
            text.AppendLine("Best Regards,");
            text.AppendLine("MobilityOne Sdn Bhd");
            return text.ToString();
        }
    }
}

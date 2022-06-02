using Microsoft.Extensions.Logging;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Data.Entities.Notification;
using OneRegister.Data.Identication;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.MerchantRegistration;
using System.Collections.Generic;
using System.Text;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.NotificationFactory.Makers
{
    public class OP2AcceptedNotifMaker: INotificationMaker
    {
        private readonly MerchantService _merchantService;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;
        private readonly ILogger<OP2AcceptedNotifMaker> _logger;

        public OP2AcceptedNotifMaker(
            MerchantService merchantService,
            UserService userService,
            NotificationService notificationService,
            ILogger<OP2AcceptedNotifMaker> logger)
        {
            _merchantService = merchantService;
            _userService = userService;
            _notificationService = notificationService;
            _logger = logger;
        }
        public bool IsEligible(ActionType actionType)
        {
            return actionType == ActionType.OP2Accepted;
        }

        public void Make(NotificationJob notificationJob)
        {
            try
            {
                var merchant = _merchantService.GetAsAdmin(notificationJob.RefId.Value, false, m => m.MerchantInfo);
                List<OUser> users = new();
                users.AddRange(_userService.GetUsersInRole(BasicRoles.MerchantRiskHead.name));

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
                        Subject = $"OneRegister Merchant Notification: {merchant.MerchantInfo.Name} accepted by OP2",
                        Message = CreateEmailMessage(merchant,user)
                    });
                }

                _notificationService.AddTaskRange(tasks);
                _notificationService.JobDone(notificationJob.Id);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Failed to create task for this job id {notificationJob.Id}");
                _notificationService.JobFail(notificationJob.Id, ex.Message);
            }
        }

        private static string CreateEmailMessage(Merchant merchant,OUser user )
        {
            var text = new StringBuilder();
            text.AppendLine($"Dear {user.Name},");
            text.AppendLine("You have a message from OneRegister Merchant Portal:");
            text.AppendLine();
            text.AppendLine($"Merchant name: {merchant.MerchantInfo.Name}");
            text.AppendLine("Action by Risk: Accept or Reject");
            text.AppendLine();
            text.AppendLine("Thank you.");
            text.AppendLine();
            text.AppendLine("Best Regards,");
            text.AppendLine("MobilityOne Sdn Bhd");
            return text.ToString();
        }
    }
}

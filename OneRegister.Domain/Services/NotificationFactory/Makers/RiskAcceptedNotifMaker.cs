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
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.NotificationFactory.Makers
{
    public class RiskAcceptedNotifMaker : INotificationMaker
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<RiskAcceptedNotifMaker> _logger;
        private readonly MerchantService _merchantService;
        private readonly UserService _userService;

        public RiskAcceptedNotifMaker(
            NotificationService notificationService,
            ILogger<RiskAcceptedNotifMaker> logger,
            MerchantService merchantService,
            UserService userService
            )
        {
            _notificationService = notificationService;
            _logger = logger;
            _merchantService = merchantService;
            _userService = userService;
        }
        public bool IsEligible(ActionType actionType)
        {
            return actionType == ActionType.RiskAccepted;
        }

        public void Make(NotificationJob notificationJob)
        {
            try
            {
                var merchant = _merchantService.GetAsAdmin(notificationJob.RefId.Value, false, m => m.MerchantInfo);
                List<OUser> users = new();

                users.AddRange(_userService.GetUsersInRole(BasicRoles.MerchantOPLvl1.name));
                users.AddRange(_userService.GetUsersInRole(BasicRoles.MerchantOPLvl2.name));
                users.AddRange(_userService.GetUsersInRole(BasicRoles.MerchantRiskHead.name));

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
                        Subject = $"OneRegister Merchant Notification: Congratulations! {merchant.MerchantInfo.Name} application has been approved",
                        Message = CreateEmailMessage(merchant,user)
                    });
                }

                //OneCall API task
                tasks.Add(new NotificationTask()
                {
                    Name = "OneCall API",
                    NotificationJobId = notificationJob.Id,
                    NotificationType = NotificationType.MerchantOnboardAPI,
                    State = StateOfEntity.InProgress,
                    Message = $"merchant {merchant.Name} ({merchant.MerchantInfo.FormNumber}) is ready to fetch"
                });

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
            text.AppendLine($"Dear {user.Name}");
            text.AppendLine("You have a message from OneRegister Merchant Portal:");
            text.AppendLine();
            text.AppendLine($"Merchant name: {merchant.MerchantInfo.Name}");
            text.AppendLine($"Congratulations! {merchant.MerchantInfo.Name} application has been approved!");
            text.AppendLine();
            text.AppendLine("Thank you.");
            text.AppendLine();
            text.AppendLine("Best Regards,");
            text.AppendLine("MobilityOne Sdn Bhd");
            return text.ToString();
        }
    }
}

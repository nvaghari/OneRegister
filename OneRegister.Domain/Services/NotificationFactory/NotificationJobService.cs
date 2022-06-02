using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.Notification;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Services.NotificationFactory
{
    public class NotificationJobService
    {
        private readonly IGenericRepository<NotificationJob> _notificationJobRepository;

        public NotificationJobService(IGenericRepository<NotificationJob> notificationJobRepository)
        {
            _notificationJobRepository = notificationJobRepository;
        }
        public Guid Register(ActionType actionType, Guid? refId, string refName)
        {
            NotificationJob notif = new()
            {
                ActionType = actionType,
                RefId = refId,
                Name = refName
            };
            _notificationJobRepository.Add(notif);
            return notif.Id;
        }

        internal List<NotificationJob> GetInProgressJobs()
        {
            return _notificationJobRepository
                .Entities.AsNoTracking()
                .Where(n => n.State == StateOfEntity.InProgress)
                .ToList(); 
        }

        public bool IsContextConnected()
        {
            return _notificationJobRepository.Context.Database.CanConnect();
        }
    }
}

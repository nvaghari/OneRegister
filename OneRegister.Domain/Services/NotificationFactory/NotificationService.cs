using OneRegister.Data.Contract;
using OneRegister.Data.Entities.Notification;
using System;
using System.Collections.Generic;

namespace OneRegister.Domain.Services.NotificationFactory
{
    public class NotificationService
    {
        private readonly IGenericRepository<NotificationTask> _notifTaskRepository;
        private readonly IGenericRepository<NotificationJob> _notifJobRepository;

        public NotificationService(
            IGenericRepository<NotificationTask> notifTaskRepository,
            IGenericRepository<NotificationJob> notifJobRepository)
        {
            _notifTaskRepository = notifTaskRepository;
            _notifJobRepository = notifJobRepository;
        }
        public bool IsContextConnected()
        {
            return _notifTaskRepository.Context.Database.CanConnect();
        }

        public void AddTaskRange(List<NotificationTask> notificationTasks)
        {
            _notifTaskRepository.Add(notificationTasks);
        }

        internal List<NotificationTask> GetInProgressTasks()
        {
            return _notifTaskRepository.GetByState(StateOfEntity.InProgress);
        }

        internal void JobDone(Guid id)
        {
            var notificationJob = _notifJobRepository.GetById(id);
            notificationJob.State = StateOfEntity.Complete;
            notificationJob.Result = "successful";
            _notifJobRepository.Update(notificationJob);
        }

        internal void JobFail(Guid id, string message)
        {
            var notificationJob = _notifJobRepository.GetById(id);
            notificationJob.State = StateOfEntity.Fail;
            notificationJob.Result = message;
            _notifJobRepository.Update(notificationJob);
        }
        internal void TaskDone(Guid id)
        {
            NotificationTask task = _notifTaskRepository.GetById(id);
            task.State = StateOfEntity.Complete;
            task.Result = "Done";
            _notifTaskRepository.Update(task);
        }
        internal void TaskFail(Guid id, string message)
        {
            var task = _notifTaskRepository.GetById(id);
            task.State = StateOfEntity.Fail;
            task.Result = message;
            _notifTaskRepository.Update(task);
        }
    }
}

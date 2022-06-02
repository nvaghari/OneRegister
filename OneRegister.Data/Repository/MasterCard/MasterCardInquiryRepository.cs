using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Repository.MasterCard
{
    public class MasterCardInquiryRepository
    {
        public MasterCardInquiryRepository(OneRegisterContext context)
        {
            Context = context;
        }

        public OneRegisterContext Context { get; }

        public void AddInquiries(IEnumerable<InquiryTask> tasks)
        {
            var taskList = tasks.Select(t => UpdateCreation(t)).ToList();
            foreach (var task in taskList)
            {
                if (Context.InquiryTasks.Any(t => t.RefId == task.RefId && t.InquiryType == task.InquiryType)) continue;
                Context.InquiryTasks.Add(task);
            }
            //Context.InquiryTasks.AddRange(taskList);
            Context.SaveChanges();
        }

        public List<InquiryTask> GetInProgressInquiryTasks()
        {
            return Context.InquiryTasks.Where(t => t.State == StateOfEntity.InProgress).ToList();
        }

        public static TEntity UpdateCreation<TEntity>(TEntity entity) where TEntity : IBaseEntity
        {
            entity.CreatedBy = entity.ModifiedBy = Constants.BasicUser.AdminId;
            if (entity.State == StateOfEntity.Init)
            {
                entity.State = StateOfEntity.InProgress;
            }
            return entity;
        }
        public static TEntity UpdateModification<TEntity>(TEntity entity) where TEntity : IBaseEntity
        {
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = Constants.BasicUser.AdminId;

            return entity;
        }
    }
}

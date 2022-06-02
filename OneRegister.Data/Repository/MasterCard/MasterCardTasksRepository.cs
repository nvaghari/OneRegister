using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Repository.MasterCard
{
    public class MasterCardTasksRepository
    {
        public MasterCardTasksRepository(OneRegisterContext context)
        {
            Context = context;
        }

        public OneRegisterContext Context { get; }

        public void MarkAsFailure(Guid taskId,string source, string resutl)
        {
            var task = Context.InquiryTasks.Find(taskId);
            task.ErrorSource = source;
            task.Result = resutl.Truncate(1000);
            task.State = StateOfEntity.Fail;
            task.ModifiedAt = DateTime.Now;

            Context.InquiryTasks.Update(task);
            Context.SaveChanges();
        }
        public void MarkAsFailure(Guid taskId, string source,string code, string resutl)
        {
            var task = Context.InquiryTasks.Find(taskId);
            task.ErrorSource = source;
            task.ErrorCode = code;
            task.Result = resutl.Truncate(1000);
            task.State = StateOfEntity.Fail;
            task.ModifiedAt = DateTime.Now;

            Context.InquiryTasks.Update(task);
            Context.SaveChanges();
        }
        public void MarkAsSuccess(Guid taskId)
        {
            var task = Context.InquiryTasks.Find(taskId);
            task.Result = "Success";
            task.State = StateOfEntity.Complete;
            task.ModifiedAt = DateTime.Now;

            Context.InquiryTasks.Update(task);
            Context.SaveChanges();
        }

        public Entities.MasterCard.InquiryTask GetByRefId2(string refId2)
        {
            return Context.InquiryTasks.Where(t => t.RefId2 == refId2).FirstOrDefault();
        }

        public void MarkAsFetched(Guid taskId, string refId2)
        {
            var task = Context.InquiryTasks.Find(taskId);
            task.Result = "Success";
            task.RefId2 = refId2;
            task.State = StateOfEntity.Fetched;
            task.ModifiedAt = DateTime.Now;

            Context.InquiryTasks.Update(task);
            Context.SaveChanges();
        }

        public IEnumerable<Entities.MasterCard.InquiryTask> RetrieveTasksForList(string searchValue, int start, int take, out int total, out int count)
        {
            var query = Context.InquiryTasks.AsNoTracking();
            total = query.Count();
            query = query
                .Where(a =>
                            string.IsNullOrEmpty(searchValue)
                            || a.RefId.Contains(searchValue));

            count = query.Count();
            query = query.OrderByDescending(a => a.ModifiedAt);
            query = query.Skip(start).Take(take);

            return query;
        }
    }
}

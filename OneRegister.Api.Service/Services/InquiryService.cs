using OneRegister.Api.Service.Abstract.Services;
using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Api.Service.Services
{
    public class InquiryService : IInquiryService
    {
        public InquiryService(OneRegisterContext oneRegisterContext)
        {
            Context = oneRegisterContext;
        }

        public OneRegisterContext Context { get; }

        public async Task<Guid> ChangeStatusToInProgress(string refId, string inquiryType)
        {
            var isTypeValid = Enum.TryParse<InquiryType>(inquiryType, true, out var typeResult);
            if (!isTypeValid)
            {
                throw new ApplicationException($"the inquiry type is not valid: {inquiryType}");
            }
            var inquiryTask = Context.InquiryTasks.FirstOrDefault(i => i.RefId == refId && i.InquiryType == typeResult);
            if (inquiryTask == null)
            {
                throw new ApplicationException($"the inquiry task does not exist, refid: {refId} type: {inquiryType}");
            }

            inquiryTask.State = StateOfEntity.InProgress;
            inquiryTask.Result = null;
            inquiryTask.ErrorSource = null;
            inquiryTask.ErrorCode = null;
            inquiryTask.ModifiedAt = DateTime.Now;
            //TODO add user as ModifiedBy
            await Context.SaveChangesAsync();

            return inquiryTask.Id;
        }
    }
}

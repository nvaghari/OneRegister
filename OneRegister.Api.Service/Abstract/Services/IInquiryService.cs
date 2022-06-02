using OneRegister.Data.Context;
using System;
using System.Threading.Tasks;

namespace OneRegister.Api.Service.Abstract.Services
{
    public interface IInquiryService
    {
        OneRegisterContext Context { get; }

        Task<Guid> ChangeStatusToInProgress(string refId, string inquiryType);
    }
}
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Model;
using System;

namespace OneRegister.Domain.Extentions
{
    public static class ExceptionExtension
    {
        public static PersistResult ToPersistResult(this Exception exception, PersistResult result)
        {
            result.IsSuccessful = false;
            result.Errors.Add(exception.Message);
            if (exception.InnerException != null)
            {
                result.Errors.Add(exception.InnerException.Message);
            }
            return result;
        }

        public static FullResponse ToFullResponse(this Exception exception)
        {
            while(exception.InnerException != null) exception = exception.InnerException;
            return new FullResponse
            {
                IsSuccessful = false,
                Message = exception.Message 
            };

        }
        public static SimpleResponse ToSimpleResponse(this Exception exception)
        {
            return new SimpleResponse
            {
                IsSuccessful = false,
                Message = exception.Message
            };

        }
    }
}

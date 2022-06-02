using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Model;
using System.Linq;

namespace OneRegister.Domain.Extentions
{
    public static class PersistResultExtension
    {
        public static FullResponse ToFullResponse(this PersistResult persistResult)
        {
            var result = new FullResponse();
            if (persistResult.IsSuccessful)
            {
                result.IsSuccessful = true;
                result.Id = persistResult.Id.ToString();
                return result;
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = persistResult.Errors.Aggregate((a, b) => a + " " + b);
                return result;
            }
        }
    }
}

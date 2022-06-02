using Microsoft.AspNetCore.Identity;
using OneRegister.Core.Model.ControllerResponse;
using System.Linq;

namespace OneRegister.Domain.Extentions
{
    public static class IdentityResultExtension
    {
        public static FullResponse ToFullResponse(this IdentityResult result)
        {
            return new FullResponse
            {
                IsSuccessful = false,
                Message = result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b)
            };
        }
    }
}

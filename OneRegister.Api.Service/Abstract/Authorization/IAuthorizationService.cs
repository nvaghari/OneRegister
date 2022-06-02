using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Api.Service.Abstract.Authorization
{
    public interface IAuthorizationService
    {
        Task<string> GetTokenAsync(string userName, string userKey);
        string ValidateToken(string token);
        Task<bool> IsUserValid(string userName);
    }
}

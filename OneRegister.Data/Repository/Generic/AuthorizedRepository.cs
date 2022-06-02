using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Security.Services.HttpContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Repository.Generic
{
    public class AuthorizedRepository<T> : GenericRepository<T>,IAuthorizedRepository<T> where T : class, IGenericEntity
    {
        private readonly IHttpContextPermissionHandler _permissionHandler;

        public AuthorizedRepository(OneRegisterContext context, IHttpContextPermissionHandler permissionHandler) : base(context)
        {
            _permissionHandler = permissionHandler;
        }

        public override Guid? CurrentUserId => _permissionHandler.UserId();
    }
}

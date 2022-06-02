using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneRegister.Api.Service.Abstract.Authorization;
using OneRegister.Api.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Api.MasterCard.Filters
{
    internal class TokenAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly IAuthorizationService _authorizationService;

        public TokenAuthorizationAttribute(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization") ||
                    !context.HttpContext.Request.Headers["Authorization"].First().StartsWith("Bearer "))
                {
                    throw new AuthorizationException("header doesn't have Bearer authorization token");
                }
                var token = context.HttpContext.Request.Headers["Authorization"]
                    .First()
                    .Substring("Bearer ".Length);
                var user = _authorizationService.ValidateToken(token);
                if (!_authorizationService.IsUserValid(user).Result)
                {
                    throw new AuthorizationException("user does not have the required Role");
                }

                base.OnActionExecuting(context);

            }
            catch (AuthorizationException ex)
            {

                context.Result = new UnauthorizedResult();
            }
        }
    }
}

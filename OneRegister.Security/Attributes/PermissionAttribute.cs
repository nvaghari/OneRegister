using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OneRegister.Security.Services;
using System;
using System.Security.Claims;

namespace OneRegister.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PermissionAttribute : ActionFilterAttribute,IAuthorizationFilter,IFilterFactory, IPermissionAttribute
    {
        private readonly IPermissionService _permissionService;
        public PermissionAttribute(string Id, string Name, string OrganizationId)
        {
            this.Id = Id;
            this.Name = Name;
            this.OrganizationId = OrganizationId;
        }
        public PermissionAttribute(IPermissionService permissionService, string id, string name, string organizationId)
        {
            _permissionService = permissionService;
            Id = id;
            Name = name;
            OrganizationId = organizationId;
        }
        private Guid _guid => Guid.Parse(Id);
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string OrganizationId { get; private set; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var permissionservice =(IPermissionService) serviceProvider.GetService(typeof(IPermissionService));
            return new PermissionAttribute(permissionservice,Id,Name,OrganizationId);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!_permissionService.IsAuthenticated(context.HttpContext.User, _guid))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}

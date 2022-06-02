using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace OneRegister.Api.MasterCard.Filters
{
    public class OpenApiAuthorizationHeaderAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && descriptor.ControllerName.Contains("Token"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "username",
                    In = ParameterLocation.Header,
                    Description = "username",
                    Required = true
                });
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "userkey",
                    In = ParameterLocation.Header,
                    Description = "userkey",
                    Required = true
                });
            }
        }
    }
}

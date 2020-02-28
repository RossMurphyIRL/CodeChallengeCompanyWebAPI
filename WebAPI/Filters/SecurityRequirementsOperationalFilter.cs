

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {

            if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
                !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
            {
				if (operation.Parameters == null)
					operation.Parameters = new List<OpenApiParameter>();

				operation.Parameters.Add(new OpenApiParameter
				{
					Name = "Authorization",
					In = ParameterLocation.Path,
					Description = "access token",
					Required = true,
					Schema = new OpenApiSchema()
					{
						Type= "string"
					}
				});
			}
        }
    }
}

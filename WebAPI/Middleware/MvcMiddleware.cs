using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;

namespace WebAPI.Middleware
{
    public static class MvcMiddleware
    {
        public static void ConfigureMvc(this IServiceCollection services)
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

    }
}

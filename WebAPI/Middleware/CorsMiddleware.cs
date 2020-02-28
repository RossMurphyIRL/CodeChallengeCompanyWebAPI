﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public static class CorsMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedUrls = configuration
                .GetSection("Cors")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();

            services.AddCors(options =>
            {
                options.AddPolicy("DevCorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(allowedUrls)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void ConfigureCors(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseCors(env.IsDevelopment() ? "DevCorsPolicy" : "CorsPolicy");
        }
    }
}

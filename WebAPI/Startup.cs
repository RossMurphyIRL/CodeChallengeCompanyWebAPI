using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure;
using Core.Interfaces;
using WebAPI.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;
using Services.Services;
using Services.Interfaces;
using FluentValidation;
using Services.Validators;
using Core;

namespace CodeChallenege
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors(Configuration);
            // Use in-memory database if no sql environment available otherwise use connect to db from connection string.
            if (Configuration.GetValue<bool>("UseInMemoryDB"))
            {
                services.AddDbContext<CompanyContext>(opt =>
                               opt.UseInMemoryDatabase("CompanyList"));
            } else
            {
                services.AddDbContext<CompanyContext>(opt =>
                         opt.UseSqlServer(Configuration.GetConnectionString("CodeChallenegeDatabase")));
            }
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation();
            services.AddTransient<IValidator<CompanyDto>, CompanyValidator>();
            services.AddTransient<IValidator<UserDto>, UserValidator>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyContext, CompanyRepository>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddControllers();
            services.ConfigureSwagger();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureSwagger(env, Configuration.GetValue<bool>("UseSwagger"));
            app.ConfigureExceptionHandler();
            app.UseStaticFiles();
            app.ConfigureCors(env);
            app.UseSecurityHeaders();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

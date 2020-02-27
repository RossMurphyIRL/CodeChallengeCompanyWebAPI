using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace WebAPI.Middleware
{
	/// <summary>
	/// </summary>
	public static class SwaggerMiddleware
	{
		/// <summary>
		/// Register the Swagger generator, defining 1 or more Swagger documents
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureSwagger(this IServiceCollection services)
		{
			var binaryPath =
				$@"{
						Path.GetDirectoryName(
							Assembly.GetEntryAssembly().Location)
					}\solutionsettings.json";

			var solutionSettings = JObject.Parse(File.ReadAllText(binaryPath));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Company API",
					Description = "Glass Lewis Code Challenege",
					Contact = new OpenApiContact
					{
						Name = "Ross Murphy",
						Email = "rossbmurphy@gmail.com"
					}
				});	
			});
		}

		/// <summary>
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="useSwagger"></param>
		public static void ConfigureSwagger(this IApplicationBuilder app, IWebHostEnvironment env, bool useSwagger)
		{
			if (!env.IsDevelopment() && !useSwagger) return;

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API V1");
			});
		}
	}
}
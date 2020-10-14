using GroupBudget.Account.WebApi;
using GroupBudget.Clearance.Business.WebApi;
using GroupBudget.WebApi.Filters;
using GroupBudget.WebApi.Infrastructure;
using GroupBudget.WebApi.Middleware;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace GroupBudget.WebApi
{
	public class Startup
	{
		private readonly IConfiguration configuration;

		public Startup(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMediatrOnUseCases();

			services.AddHangfire(config =>
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseMemoryStorage()
			);

			services.AddHangfireServer();

			services.AddAccount(configuration);
			services.AddClearance(configuration);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "GroupBudget API", Version = "v1" });
			});

			services.AddControllers(options =>
			{
				options.Filters.Add(typeof(TrackActionPerformanceFilter));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseApiExceptionHandler(options => options.AddResponseDetails = UodateApiErrorResponse);
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroupBudget v1");
			});

			//app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHangfireDashboard("/admin/hangfire", new DashboardOptions { Authorization = new[] { new HangFireAuthorizationFilter() } });
			});
		}

		private static void UodateApiErrorResponse(HttpContext context, Exception ex, ApiError error)
		{
			//do something
		}
	}
}
using Codefondo.UseCase.Kernel;
using EventStore.ClientAPI;
using GroupBudget.Clearance.Business.Domain;
using GroupBudget.Clearance.Business.Persistence;
using GroupBudget.Clearance.Business.Persistence.MongoDb;
using GroupBudget.Clearance.Business.Services;
using GroupBudget.Clearance.Business.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace GroupBudget.Clearance.Business.WebApi
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddClearance(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ClearanceDatabaseSettings>(configuration.GetSection("ClearanceDtosDatabaseSettings"));
			services.AddSingleton<IClearanceDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ClearanceDatabaseSettings>>().Value);

			var esConnection = EventStoreConnection.Create(
									ConnectionSettings.Create().KeepReconnecting().DisableServerCertificateValidation().DisableTls(),
									new System.Uri(configuration["ClearanceEventStoreSettings:ConnectionString"]));
			esConnection.ConnectAsync().Wait();

			services.AddSingleton(esConnection);

			services.AddTransient<IRepository<ClearanceRoot>, Repository<ClearanceRoot>>();
			services.AddTransient<IClearanceDtoRepository, ClearanceDtoRepository>();
			services.AddTransient<IUserSettingsService, UserSettingsService>();
			services.AddTransient<IAccountDtoService, AccountDtoService>();

			services.AddMvcCore().AddApplicationPart(Assembly.GetExecutingAssembly()).AddControllersAsServices();

			return services;
		}
	}
}
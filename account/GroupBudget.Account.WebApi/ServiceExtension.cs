using Codefondo.UseCase.Kernel;
using EventStore.ClientAPI;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Persistence;
using GroupBudget.Account.Persistence.MongoDb;
using GroupBudget.Account.UseCases;
using GroupBudget.Account.UseCases.EventHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace GroupBudget.Account.WebApi
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddAccount(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AccountDatabaseSettings>(configuration.GetSection("AccountDtosDatabaseSettings"));
			services.AddSingleton<IAccountDatabaseSettings>(sp => sp.GetRequiredService<IOptions<AccountDatabaseSettings>>().Value);

			var esConnection = EventStoreConnection.Create(
									ConnectionSettings.Create().KeepReconnecting().DisableServerCertificateValidation().DisableTls(), 
									new System.Uri(configuration["AccountEventStoreSettings:ConnectionString"]));
			esConnection.ConnectAsync().Wait();

			services.AddSingleton(esConnection);

			services.AddTransient<IRepository<AccountRoot>, Repository<AccountRoot>>();
			services.AddTransient<IAccountDtoRepository, AccountDtoRepository>();

			services.AddMvcCore().AddApplicationPart(Assembly.GetExecutingAssembly()).AddControllersAsServices();

			return services;
		}
	}
}
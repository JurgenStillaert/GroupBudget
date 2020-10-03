using GroupBudget.Account.Persistence;
using GroupBudget.Account.Persistence.MongoDb;
using GroupBudget.Account.UseCases;
using GroupBudget.Account.UseCases.EventHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GroupBudget.Account.Infrastructure
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddAccount(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AccountDatabaseSettings>(configuration.GetSection("AccountDtosDatabaseSettings"));
			services.AddSingleton<IAccountDatabaseSettings>(sp => sp.GetRequiredService<IOptions<AccountDatabaseSettings>>().Value);

			services.AddTransient<IAccountRepository, AccountRepository>();
			services.AddTransient<IAccountDtoRepository, AccountDtoRepository>();

			return services;
		}
	}
}
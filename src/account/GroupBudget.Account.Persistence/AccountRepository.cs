using Codefondo.DDD.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.UseCases;
using System.Threading.Tasks;

namespace GroupBudget.Account.Persistence
{
	public class AccountRepository : IAccountRepository
	{
		public Task<AggregateRoot> Load()
		{
			return Task.FromResult<AggregateRoot>(null);
		}

		public Task Save(AccountRoot aggregateRoot)
		{
			return Task.CompletedTask;
		}
	}
}
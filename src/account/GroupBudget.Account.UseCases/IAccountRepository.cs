using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;

namespace GroupBudget.Account.UseCases
{
	public interface IAccountRepository : IRepository<AccountRoot>
	{
	}
}
using GroupBudget.Account.Messages.Queries;
using GroupBudget.Clearance.Business.UseCases;
using MediatR;
using System;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.Services
{
	public class AccountDtoService : IAccountDtoService
	{
		private readonly IMediator mediator;

		public AccountDtoService(IMediator mediator)
		{
			this.mediator = mediator;
		}

		public async Task<TotalSpent> TotalAmountSpentOnAccount(Guid accountId)
		{
			var account = await mediator.Send(new GetAccountDtoByAccountIdQuery(accountId));

			if (account == null)
			{
				throw new  InvalidOperationException("Cannot close an account in clearance that does not exist");
			}

			return new TotalSpent(account.TotalSpent, account.CurrencyCode);
		}
	}
}
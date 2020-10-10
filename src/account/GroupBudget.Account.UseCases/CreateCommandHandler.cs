using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Commands;
using GroupBudget.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class CreateCommandHandler : CreateCommandHandler<CreateCommand, AccountRoot>
	{
		public CreateCommandHandler(IRepository<AccountRoot> accountRepo, IMediator mediator)
			: base(accountRepo, mediator)
		{
		}

		private void PreHandle(CreateCommand command)
		{
			AggregateId = AccountId.FromGuid(command.AccountDto.Id);
		}

		protected async override Task<AccountRoot> Apply(CreateCommand command, CancellationToken cancellationToken)
		{
			var account = AccountRoot.Create(
								AccountId.FromGuid(command.AccountDto.Id),
								UserId.FromGuid(command.AccountDto.OwnerId),
								Period.FromMonth(command.AccountDto.Year, command.AccountDto.Month),
								CurrencyCode.FromString(command.AccountDto.Currency));

			return await Task.FromResult(account);
		}
	}
}
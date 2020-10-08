using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	public sealed class CreateCommand : IRequest<Unit>
	{
		private readonly CreateAccountDto accountDto;

		public CreateCommand(CreateAccountDto accountDto)
		{
			this.accountDto = accountDto;
		}

		internal sealed class CreateCommandHandler : CreateCommandHandler<CreateCommand, AccountRoot>
		{
			public CreateCommandHandler(IRepository<AccountRoot> accountRepo, IMediator mediator)
				: base(accountRepo, mediator)
			{
			}

			private void PreHandle(CreateCommand command)
			{
				AggregateId = AccountId.FromGuid(command.accountDto.Id);
			}

			protected async override Task<AccountRoot> Apply(CreateCommand command, CancellationToken cancellationToken)
			{
				var account = AccountRoot.Create(
									AccountId.FromGuid(command.accountDto.Id),
									UserId.FromGuid(command.accountDto.OwnerId),
									Period.FromMonth(command.accountDto.Year, command.accountDto.Month),
									CurrencyCode.FromString(command.accountDto.Currency));

				return account;
			}
		}
	}
}
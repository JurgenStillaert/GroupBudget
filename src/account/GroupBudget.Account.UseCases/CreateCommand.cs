using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Dtos;
using MediatR;

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
			private readonly IAccountRepository accountRepo;

			public CreateCommandHandler(IAccountRepository accountRepo, IMediator mediator)
				: base(accountRepo, mediator)
			{
				this.accountRepo = accountRepo;
			}

			private void PreHandle(CreateCommand command)
			{
				AggregateId = AccountId.FromGuid(command.accountDto.Id);
			}

			private AccountRoot Handle(CreateCommand command)
			{
				var account = AccountRoot.Create(
									AccountId.FromGuid(command.accountDto.Id),
									UserId.FromGuid(command.accountDto.OwnerId),
									Period.FromMonth(command.accountDto.Year, command.accountDto.Month),
									CurrencyCode.FromString(command.accountDto.Currency));

				accountRepo.Save(account);

				return account;
			}
		}
	}
}
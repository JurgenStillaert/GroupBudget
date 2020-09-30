using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using MediatR;
using System;

namespace GroupBudget.Account.UseCases
{
	public sealed class CreateCommand : IRequest<Unit>
	{
		public Guid Id { get; }
		public Guid OwnerId { get; }
		public int Month { get; }
		public int Year { get; }
		public string Currency { get; }

		public CreateCommand(Guid id, Guid ownerId, int month, int year, string currency)
		{
			Id = id;
			OwnerId = ownerId;
			Month = month;
			Year = year;
			Currency = currency;
		}

		internal sealed class CreateCommandHandler : CreateCommandHandler<CreateCommand, AccountRoot>
		{
			private readonly IAccountRepository accountRepo;

			public CreateCommandHandler(IAccountRepository accountRepo)
				: base(accountRepo)
			{
				this.accountRepo = accountRepo;
			}

			private void PreHandle(CreateCommand command)
			{
				AggregateId = AccountId.FromGuid(command.Id);
			}

			private AccountRoot Handle(CreateCommand command)
			{
				return AccountRoot.Create(
									AccountId.FromGuid(command.Id),
									UserId.FromGuid(command.OwnerId),
									Period.FromMonth(command.Year, command.Month),
									CurrencyCode.FromString(command.Currency));
			}
		}
	}
}
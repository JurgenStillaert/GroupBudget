using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	public sealed class CloseAccountCommand : IRequest<Unit>
	{
		private readonly Guid accountId;

		public CloseAccountCommand(Guid accountId)
		{
			this.accountId = accountId;
		}

		internal sealed class CloseAccountCommandHandler : UpdateCommandHandler<CloseAccountCommand, AccountRoot>
		{
			public CloseAccountCommandHandler(IRepository<AccountRoot> accountRepo, IMediator mediator)
				: base(accountRepo, mediator)
			{
			}

			protected override void PreHandle(CloseAccountCommand command)
			{
				AggregateId = AccountId.FromGuid(command.accountId);
			}

			protected async override Task<AccountRoot> Apply(CloseAccountCommand command, CancellationToken cancellationToken)
			{
				AggregateRoot.Close();

				return AggregateRoot;
			}
		}
	}
}
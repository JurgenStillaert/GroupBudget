using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class CloseAccountCommandHandler : UpdateCommandHandler<CloseAccountCommand, AccountRoot>
	{
		public CloseAccountCommandHandler(IRepository<AccountRoot> accountRepo, IMediator mediator)
			: base(accountRepo, mediator)
		{
		}

		protected override void PreHandle(CloseAccountCommand command)
		{
			AggregateId = AccountId.FromGuid(command.AccountId);
		}

		protected async override Task<AccountRoot> Apply(CloseAccountCommand command, CancellationToken cancellationToken)
		{
			AggregateRoot.Close();

			return await Task.FromResult(AggregateRoot);
		}
	}
}
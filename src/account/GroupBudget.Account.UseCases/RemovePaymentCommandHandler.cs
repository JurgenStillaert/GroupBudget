using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class RemovePaymentCommandCommandHandler : UpdateCommandHandler<RemovePaymentCommand, AccountRoot>
	{
		public RemovePaymentCommandCommandHandler(IRepository<AccountRoot> repo, IMediator mediator) : base(repo, mediator)
		{
		}

		protected override void PreHandle(RemovePaymentCommand command)
		{
			AggregateId = AccountId.FromGuid(command.AccountRootId);
		}

		protected async override Task<AccountRoot> Apply(RemovePaymentCommand command, CancellationToken cancellationToken)
		{
			AggregateRoot.DeleteBooking(Domain.BookingId.FromGuid(command.BookingId));

			return await Task.FromResult(AggregateRoot);
		}
	}
}
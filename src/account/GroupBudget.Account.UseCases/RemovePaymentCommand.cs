using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	public sealed class RemovePaymentCommand : IRequest<Unit>
	{
		public RemovePaymentCommand(Guid accountRootId, Guid bookingId)
		{
			AccountRootId = accountRootId;
			BookingId = bookingId;
		}

		public Guid AccountRootId { get; }
		public Guid BookingId { get; }
		public AccountItemDto AccountItemDto { get; }

		public class RemovePaymentCommandCommandHandler : UpdateCommandHandler<RemovePaymentCommand, AccountRoot>
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

				return AggregateRoot;
			}
		}
	}
}
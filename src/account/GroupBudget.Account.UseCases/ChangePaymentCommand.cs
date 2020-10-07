using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Dtos;
using GroupBudget.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	public sealed class ChangePaymentCommand : IRequest<Unit>
	{
		public ChangePaymentCommand(Guid accountRootId, Guid bookingId, AccountItemDto accountItemDto)
		{
			AccountRootId = accountRootId;
			BookingId = bookingId;
			AccountItemDto = accountItemDto;
		}

		public Guid AccountRootId { get; }
		public Guid BookingId { get; }
		public AccountItemDto AccountItemDto { get; }

		public class ChangePaymentCommandHandler : UpdateCommandHandler<ChangePaymentCommand, AccountRoot>
		{
			public ChangePaymentCommandHandler(IRepository<AccountRoot> repo, IMediator mediator) : base(repo, mediator)
			{
			}

			protected override void PreHandle(ChangePaymentCommand command)
			{
				AggregateId = AccountId.FromGuid(command.AccountRootId);
			}

			protected async override Task<AccountRoot> Apply(ChangePaymentCommand command, CancellationToken cancellationToken)
			{
				AggregateRoot.ChangeBooking(
						Domain.BookingId.FromGuid(command.BookingId),
						Payment.FromString(command.AccountItemDto.Amount, AggregateRoot.Currency.Value),
						BookingDate.FromDate(command.AccountItemDto.Date),
						Description.FromString(command.AccountItemDto.Description));

				return AggregateRoot;
			}
		}
	}
}
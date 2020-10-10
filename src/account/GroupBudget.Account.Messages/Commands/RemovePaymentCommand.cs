using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;

namespace GroupBudget.Account.Messages.Commands
{
	public sealed class RemovePaymentCommand : IRequest
	{
		public RemovePaymentCommand(Guid accountRootId, Guid bookingId)
		{
			AccountRootId = accountRootId;
			BookingId = bookingId;
		}

		public Guid AccountRootId { get; }
		public Guid BookingId { get; }
		public AccountItemDto AccountItemDto { get; }
	}
}
using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;

namespace GroupBudget.Account.Messages.Commands
{
	public sealed class ChangePaymentCommand : IRequest
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
	}
}
using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;

namespace GroupBudget.Account.Messages.Commands
{
	public sealed class BookPaymentCommand : IRequest
	{
		public BookPaymentCommand(Guid accountRootId, AccountItemDto accountItemDto)
		{
			AccountRootId = accountRootId;
			AccountItemDto = accountItemDto;
		}

		public Guid AccountRootId { get; }
		public AccountItemDto AccountItemDto { get; }
	}
}
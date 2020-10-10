using MediatR;
using System;

namespace GroupBudget.Account.Messages.Commands
{
	public sealed class CloseAccountCommand : IRequest
	{
		public CloseAccountCommand(Guid accountId)
		{
			AccountId = accountId;
		}

		public Guid AccountId { get; }
	}
}
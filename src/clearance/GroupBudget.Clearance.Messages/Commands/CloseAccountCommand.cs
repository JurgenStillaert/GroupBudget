using MediatR;
using System;

namespace GroupBudget.Clearance.Messages.Commands
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
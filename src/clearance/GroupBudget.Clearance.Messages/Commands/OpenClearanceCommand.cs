using MediatR;
using System;

namespace GroupBudget.Clearance.Messages.Commands
{
	public sealed class OpenClearanceCommand : IRequest
	{
		public OpenClearanceCommand(Guid clearanceId, Guid userId, Guid accountId, DateTime startDate, DateTime endDate)
		{
			ClearanceId = clearanceId;
			UserId = userId;
			AccountId = accountId;
			StartDate = startDate;
			EndDate = endDate;
		}

		public Guid ClearanceId { get; }
		public Guid UserId { get; }
		public Guid AccountId { get; }
		public DateTime StartDate { get; }
		public DateTime EndDate { get; }
	}
}
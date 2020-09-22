using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Messages
{
	public static class Events
	{
		public static class V1
		{
			public sealed class AccountCreated : IDomainEvent
			{
				public Guid Id { get; }
				public Guid OwnerId { get; }
				public DateTime StartDate { get; }
				public DateTime EndDate { get; }

				public AccountCreated(Guid id, Guid userId, DateTime startDate, DateTime endDate)
				{
					Id = id;
					OwnerId = userId;
					StartDate = startDate;
					EndDate = endDate;
				}
			}
		}
	}
}
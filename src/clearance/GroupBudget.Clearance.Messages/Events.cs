using Codefondo.DDD.Kernel;
using System;
using System.Collections.Generic;

namespace GroupBudget.Clearance.Messages
{
	public static class Events
	{
		public static class V1
		{
			public sealed class ClearanceCreated : IDomainEvent
			{
				public ClearanceCreated(Guid clearanceId, Dictionary<Guid, Guid?> UserAccounts, DateTime startDate, DateTime endDate)
				{
					ClearanceId = clearanceId;
					this.UserAccounts = UserAccounts;
					StartDate = startDate;
					EndDate = endDate;
				}

				public Guid ClearanceId { get; }
				public Dictionary<Guid, Guid?> UserAccounts { get; }
				public DateTime StartDate { get; }
				public DateTime EndDate { get; }
			}

			public sealed class AccountClosed : IDomainEvent
			{
				public AccountClosed(Guid clearanceGuid, Guid accountId, decimal amount, string currencyCode)
				{
					ClearanceId = clearanceGuid;
					AccountId = accountId;
					Amount = amount;
					CurrencyCode = currencyCode;
				}

				public Guid ClearanceId { get; }
				public Guid AccountId { get; }
				public decimal Amount { get; }
				public string CurrencyCode { get; }
			}

			public sealed class ClearanceFinalized : IDomainEvent
			{
				public ClearanceFinalized(Guid clearanceId, Guid payer, Guid receiver, decimal amount, string currencyCode)
				{
					ClearanceId = clearanceId;
					Payer = payer;
					Receiver = receiver;
					Amount = amount;
					CurrencyCode = currencyCode;
				}

				public Guid ClearanceId { get; }
				public Guid Payer { get; }
				public Guid Receiver { get; }
				public decimal Amount { get; }
				public string CurrencyCode { get; }
			}

			public class AccountAdded : IDomainEvent
			{
				public AccountAdded(Guid clearanceId, Guid userId, Guid accountId)
				{
					this.clearanceId = clearanceId;
					this.userId = userId;
					this.accountId = accountId;
				}

				public Guid clearanceId { get; }
				public Guid userId { get; }
				public Guid accountId { get; }
			}
		}
	}
}
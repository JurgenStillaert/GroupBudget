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
				public AccountCreated(Guid id, Guid userId, DateTime startDate, DateTime endDate, string currencyCode)
				{
					Id = id;
					OwnerId = userId;
					StartDate = startDate;
					EndDate = endDate;
					CurrencyCode = currencyCode;
				}

				public Guid Id { get; }
				public Guid OwnerId { get; }
				public DateTime StartDate { get; }
				public DateTime EndDate { get; }
				public string CurrencyCode { get; }
			}

			public sealed class BookingAddedToAccount : IDomainEvent
			{
				public BookingAddedToAccount(
					Guid accountId,
					Guid bookingId,
					decimal amount,
					string currencyCode,
					DateTime date,
					string description)
				{
					AccountId = accountId;
					BookingId = bookingId;
					Amount = amount;
					CurrencyCode = currencyCode;
					Date = date;
					Description = description;
				}

				public Guid AccountId { get; }
				public Guid BookingId { get; }
				public decimal Amount { get; }
				public string CurrencyCode { get; }
				public DateTime Date { get; }
				public string Description { get; }
			}

			public class AccountClosed : IDomainEvent
			{
				public AccountClosed(Guid id, decimal totalPaidAmount, string currencyCode)
				{
					Id = id;
					TotalPaidAmount = totalPaidAmount;
					CurrencyCode = currencyCode;
				}

				public Guid Id { get; }
				public decimal TotalPaidAmount { get; }
				public string CurrencyCode { get; }
			}

			public class BookingRemovedFromAccount : IDomainEvent
			{
				public BookingRemovedFromAccount(Guid id, Guid bookingId)
				{
					Id = id;
					BookingId = bookingId;
				}

				public Guid Id { get; }
				public Guid BookingId { get; }
			}

			public class BookingChanged : IDomainEvent
			{
				public BookingChanged(Guid id, Guid bookingId, decimal amount, string currencyCode, DateTime bookingDate, string description)
				{
					Id = id;
					BookingId = bookingId;
					Amount = amount;
					CurrencyCode = currencyCode;
					Date = bookingDate;
					Description = description;
				}

				public Guid Id { get; }
				public Guid BookingId { get; }
				public decimal Amount { get; }
				public string CurrencyCode { get; }
				public DateTime Date { get; }
				public string Description { get; }
			}
		}
	}
}
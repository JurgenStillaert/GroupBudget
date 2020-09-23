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

			public sealed class BookingAddedToAccount : IDomainEvent
			{
				public Guid BookingId { get; }
				public decimal Amount { get; }
				public string CurrencyCode { get; }
				public DateTime Date { get; }
				public string Description { get; }
				public decimal TotalAmount { get; }

				public BookingAddedToAccount(
					Guid bookingId, 
					decimal amount, 
					string currencyCode, 
					DateTime date, 
					string description)
				{
					BookingId = bookingId;
					Amount = amount;
					CurrencyCode = currencyCode;
					Date = date;
					Description = description;
				}

				public BookingAddedToAccount(BookingAddedToAccount @event, decimal totalAmount)
				{
					BookingId = @event.BookingId;
					Amount = @event.Amount;
					CurrencyCode = @event.CurrencyCode;
					Date = @event.Date;
					Description = @event.Description;
					TotalAmount = totalAmount;
				}
			}
		}
	}
}
using Codefondo.DDD.Kernel;
using GroupBudget.Account.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupBudget.Account.Domain
{
	public class Booking : Entity<BookingId>
	{
		public BookingDate Date { get; private set; }
		public Payment Payment { get; private set; }
		public Description Description { get; private set; }

		internal Booking(Events.V1.BookingAddedToAccount @event)
		{
			Id = BookingId.FromGuid(@event.BookingId);
			Date = BookingDate.FromDate(@event.Date);
			Payment = Payment.FromDecimal(@event.Amount, @event.CurrencyCode);
			Description = Description.FromString(@event.Description);
		}

		internal bool BookingDateIsInPeriod(Period period)
		{
			return Date.Value >= period.StartDate && Date.Value <= period.EndDate;
		}

		protected override void EnsureValidation() {}
	}
}

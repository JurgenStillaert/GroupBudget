using Codefondo.DDD.Kernel;
using GroupBudget.Account.Messages;
using GroupBudget.SharedKernel;

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

		protected override void EnsureValidation()
		{
			var valid = Id != default
						&& Date != default
						&& Payment != null
						&& Description != null;

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "Post-checks failed");
			}
		}

#pragma warning disable CC0068 // Unused Method
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CC0057 // Unused parameters
#pragma warning disable IDE0060 // Remove unused parameter

		private void Handle(Events.V1.BookingChanged @event)
		{
			Date = BookingDate.FromDate(@event.Date);
			Payment = Payment.FromDecimal(@event.Amount, @event.CurrencyCode);
			Description = Description.FromString(@event.Description);
		}

#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CC0057 // Unused parameters
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore CC0068 // Unused Method
	}
}
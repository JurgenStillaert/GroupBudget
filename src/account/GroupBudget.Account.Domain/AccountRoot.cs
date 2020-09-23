using Codefondo.DDD.Kernel;
using System.Collections.Generic;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Account.Domain
{
	public class AccountRoot : AggregateRoot
	{
		public UserId OwnerId { get; private set; }
		public Period Period { get; private set; }

		private IReadOnlyList<Booking> Bookings
		{
			get
			{
				return _bookings;
			}
		}

		public Money TotalAmountPaid { get; private set; }

		private List<Booking> _bookings { get; set; }

		public static AccountRoot Create(AccountId accountId, UserId userId, Period period)
		{
			var account = new AccountRoot
			{
				_bookings = new List<Booking>()
			};

			account.Apply(new V1.AccountCreated(accountId, userId, period.StartDate, period.EndDate));

			return account;
		}

		public void BookPayment(BookingId id, Payment amount, BookingDate date, Description description)
		{
			Apply(new V1.BookingAddedToAccount(id.Value, amount.Amount, amount.CurrencyCode, date.Value, description.Value));
		}

		public void Close()
		{
		}

		protected override void EnsureValidation()
		{
			var valid =
				Id != null
				&& OwnerId != null
				&& Period != null
				&& Period.StartDate != default
				&& Period.EndDate != default
				&& Period.EndDate >= Period.StartDate;

			_bookings.ForEach(x =>
			{
				var bookingDateIsValid = x.BookingDateIsInPeriod(Period);
				if (!bookingDateIsValid)
				{
					valid = false;
				}
			});

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "Post-checks failed");
			}
		}

		private void Handle(V1.AccountCreated @event)
		{
			Id = @event.Id;
			OwnerId = UserId.FromGuid(@event.OwnerId);
			Period = Period.FromStartAndEndDate(@event.StartDate, @event.EndDate);
		}

		private void Handle(V1.BookingAddedToAccount @event)
		{
			var booking = new Booking(@event);
			_bookings.Add(booking);
			//Hoe @event enrichen?
		}
	}
}
using Codefondo.DDD.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using static GroupBudget.Account.Domain.Exceptions;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Account.Domain
{
	public class AccountRoot : AggregateRoot
	{
		/// <summary>
		/// The id of the user who owns this account
		/// </summary>
		public UserId OwnerId { get; private set; }

		/// <summary>
		/// An account is delimited to a start and end date
		/// </summary>
		public Period Period { get; private set; }

		/// <summary>
		/// The state indicates if the account is open to accept bookings or closed
		/// </summary>
		public AccountState State { get; private set; }

		/// <summary>
		/// The list of the bookings already registered to the account
		/// </summary>
		public IReadOnlyList<Booking> Bookings => _bookings;

		/// <summary>
		/// The total amount paid (sum of the payments in bookings)
		/// </summary>
		public Money TotalAmountPaid => Money.FromDecimal(_bookings.Sum(x => x.Payment.Amount), Currency.Value);

		/// <summary>
		/// In which currency is this account registered
		/// </summary>
		public CurrencyCode Currency { get; private set; }

		private List<Booking> _bookings { get; set; }

		/// <summary>Create a new account</summary>
		/// <param name="accountId">ID of the account</param>
		/// <param name="userId">Owner ID of the account</param>
		/// <param name="period">The period this account is valid</param>
		/// <param name="currency">The currency of the payments registered in the bookings</param>
		public static AccountRoot Create(AccountId accountId, UserId userId, Period period, CurrencyCode currency)
		{
			var account = new AccountRoot
			{
				_bookings = new List<Booking>()
			};

			account.Apply(new V1.AccountCreated(accountId, userId, period.StartDate, period.EndDate, currency.Value));

			return account;
		}

		/// <summary>
		/// Book a new payment
		/// </summary>
		/// <param name="id">Booking Id</param>
		/// <param name="amount">Amount of the payment</param>
		/// <param name="date">Date of the booking</param>
		/// <param name="description">A short description for what this payment is made</param>
		public void BookPayment(BookingId id, Payment amount, BookingDate date, Description description)
		{
			Apply(new V1.BookingAddedToAccount(Id, id.Value, amount.Amount, amount.CurrencyCode, date.Value, description.Value));
		}

		/// <summary>
		/// Remove an existing payment
		/// </summary>
		/// <param name="id">The bookign id from the booking to remove</param>
		public void DeleteBooking(BookingId id)
		{
			Apply(new V1.BookingRemovedFromAccount(Id, id.Value));
		}

		public void ChangeBooking(BookingId id, Payment amount, BookingDate date, Description description)
		{
			Apply(new V1.BookingChanged(Id, id.Value, amount.Amount, amount.CurrencyCode, date.Value, description.Value));
		}

		/// <summary>
		/// Close this account - no bookings will be recorded anymore
		/// </summary>
		public void Close()
		{
			Apply(new V1.AccountClosed(Id, TotalAmountPaid.Amount, Currency.Value));
		}

		protected override void EnsureValidation()
		{
			var valid =
				Id != null
				&& OwnerId != null
				&& Period != null
				&& Period.StartDate != default
				&& Period.EndDate != default
				&& Period.EndDate >= Period.StartDate
				&& Currency != null;

			//All bookings need to be booked in the limits of the period of the account
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
			State = AccountState.CreateOpen();
			Currency = CurrencyCode.FromString(@event.CurrencyCode);
		}

		private void Handle(V1.BookingAddedToAccount @event)
		{
			if (State.Value == AccountState.AccountStateEnum.Closed)
			{
				throw new InvalidOperationException("Cannot add bookings to a closed account");
			}

			if (@event.CurrencyCode != Currency.Value)
			{
				throw new PaymentNotSameCurrencyAsAccountException();
			}

			var booking = new Booking(@event);
			_bookings.Add(booking);
		}

		private void Handle(V1.BookingRemovedFromAccount @event)
		{
			if (State.Value == AccountState.AccountStateEnum.Closed)
			{
				throw new InvalidOperationException("Cannot add bookings to a closed account");
			}

			var booking = _bookings.Single(x => x.Id.Value == @event.BookingId);

			_bookings.Remove(booking);
		}

		private void Handle(V1.BookingChanged @event)
		{
			if (State.Value == AccountState.AccountStateEnum.Closed)
			{
				throw new InvalidOperationException("Cannot add bookings to a closed account");
			}

			if (@event.CurrencyCode != Currency.Value)
			{
				throw new PaymentNotSameCurrencyAsAccountException();
			}

			var booking = _bookings.Single(x => x.Id.Value == @event.BookingId);

			booking.Apply(@event);
		}

#pragma warning disable CC0068 // Unused Method
#pragma warning disable CC0057 // Unused parameters

		private void Handle(V1.AccountClosed @event)
		{
			if (State.Value == AccountState.AccountStateEnum.Closed)
			{
				throw new InvalidOperationException("Cannot close an already closed account");
			}

			State = AccountState.CreateClosed();
		}

#pragma warning restore CC0057 // Unused parameters
#pragma warning restore CC0068 // Unused Method
	}
}
using GroupBudget.SharedKernel;
using NUnit.Framework;
using System;
using System.Linq;
using static Codefondo.DDD.Kernel.DomainExceptions;
using static GroupBudget.Account.Domain.Exceptions;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Account.Domain.Tests
{
	public class AccountRootTest
	{
		#region Create

		[Test]
		public void Create_ValidParams_AccountReturned()
		{
			//Arrange
			var idGuid = Guid.Parse("051f8160-ce43-4ac0-b8c2-09707c2bcda3");
			var id = AccountId.FromGuid(idGuid);
			var ownerGuid = Guid.Parse("4bc0c0e9-7181-45e4-934d-e91c0e7bbd75");
			var ownerId = UserId.FromGuid(ownerGuid);
			var period = Period.FromMonth(2020, 9);
			var currency = CurrencyCode.FromString("EUR");

			//Act
			var account = AccountRoot.Create(id, ownerId, period, currency);

			//Assert
			Assert.IsNotNull(account);
			Assert.AreEqual(idGuid, account.Id);
			Assert.AreEqual(ownerGuid, account.OwnerId.Value);
			Assert.AreEqual(DateTime.Parse("2020-09-01"), account.Period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-09-30"), account.Period.EndDate);
			Assert.AreEqual("EUR", account.Currency.Value);
			Assert.AreEqual(1, account.GetChanges().Count);
			Assert.AreEqual(typeof(V1.AccountCreated), account.GetChanges()[0].GetType());
			var @event = account.GetChanges()[0] as V1.AccountCreated;
			Assert.AreEqual(idGuid, @event.Id);
			Assert.AreEqual(ownerGuid, @event.OwnerId);
			Assert.AreEqual(DateTime.Parse("2020-09-01"), @event.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-09-30"), @event.EndDate);
			Assert.AreEqual("EUR", @event.CurrencyCode);
			Assert.AreEqual(AccountState.AccountStateEnum.Open, account.State.Value);
		}

		#endregion Create

		#region BookPayment

		[Test]
		public void BookPayment_ValidParams_BookingAddedToAccount()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			//Act
			account.BookPayment(bookingId, payment, bookingDate, description);

			//Assert
			Assert.IsNotNull(account.Bookings);
			Assert.IsNotNull(account.Bookings[0]);
			var booking = account.Bookings[0];
			Assert.AreEqual(bookingIdGuid, booking.Id.Value);
			Assert.AreEqual(100.00m, booking.Payment.Amount);
			Assert.AreEqual("EUR", booking.Payment.CurrencyCode);
			Assert.AreEqual(DateTime.Parse("2020-09-26"), booking.Date.Value);
			Assert.AreEqual("My payment", booking.Description.Value);
			Assert.AreEqual(2, account.GetChanges().Count);
			Assert.AreEqual(typeof(V1.BookingAddedToAccount), account.GetChanges()[1].GetType());
			var @event = account.GetChanges()[1] as V1.BookingAddedToAccount;
			Assert.AreEqual(accountIdGuid, @event.AccountId);
			Assert.AreEqual(bookingIdGuid, @event.BookingId);
			Assert.AreEqual(100.00m, @event.Amount);
			Assert.AreEqual("EUR", @event.CurrencyCode);
			Assert.AreEqual(DateTime.Parse("2020-09-26"), @event.Date);
			Assert.AreEqual("My payment", @event.Description);
		}

		[Test]
		public void BookPayment_PaymentDateOutsidePeriod_InvalidEntityStateException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-01-01");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			//Act & Assert
			Assert.Throws<InvalidEntityState>(() => account.BookPayment(bookingId, payment, bookingDate, description));
		}

		[Test]
		public void BookPayment_AccountClosed_InvalidOperationException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.Close();

			//Act & Assert
			Assert.Throws<InvalidOperationException>(() => account.BookPayment(bookingId, payment, bookingDate, description));
		}

		[Test]
		public void BookPayment_BookingWithDifferentCurrency_PaymentNotSameCurrencyAsAccountException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "USD");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			//Act & Assert
			Assert.Throws<PaymentNotSameCurrencyAsAccountException>(() => account.BookPayment(bookingId, payment, bookingDate, description));
		}

		#endregion BookPayment

		#region ChangeBooking

		[Test]
		public void ChangeBooking_ValidParams_BookingAddedToAccount()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var changedPayment = Payment.FromDecimal(200.00m, "EUR");
			var changedBookingDate = BookingDate.FromString("2020-09-20");
			var changedDescription = Description.FromString("My payment changed");

			//Act
			account.ChangeBooking(bookingId, changedPayment, changedBookingDate, changedDescription);

			//Assert
			Assert.IsNotNull(account.Bookings);
			Assert.IsNotNull(account.Bookings[0]);
			var booking = account.Bookings[0];
			Assert.AreEqual(bookingIdGuid, booking.Id.Value);
			Assert.AreEqual(200.00m, booking.Payment.Amount);
			Assert.AreEqual("EUR", booking.Payment.CurrencyCode);
			Assert.AreEqual(DateTime.Parse("2020-09-20"), booking.Date.Value);
			Assert.AreEqual("My payment changed", booking.Description.Value);
			Assert.AreEqual(3, account.GetChanges().Count);
			Assert.AreEqual(typeof(V1.BookingChanged), account.GetChanges()[2].GetType());
			var @event = account.GetChanges()[2] as V1.BookingChanged;
			Assert.AreEqual(accountIdGuid, @event.Id);
			Assert.AreEqual(bookingIdGuid, @event.BookingId);
			Assert.AreEqual(200.00m, @event.Amount);
			Assert.AreEqual("EUR", @event.CurrencyCode);
			Assert.AreEqual(DateTime.Parse("2020-09-20"), @event.Date);
			Assert.AreEqual("My payment changed", @event.Description);
		}

		[Test]
		public void ChangeBooking_PaymentDateOutsidePeriod_InvalidEntityStateException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var changedPayment = Payment.FromDecimal(200.00m, "EUR");
			var changedBookingDate = BookingDate.FromString("2020-01-01");
			var changedDescription = Description.FromString("My payment changed");

			//Act & Assert
			Assert.Throws<InvalidEntityState>(() => account.ChangeBooking(bookingId, changedPayment, changedBookingDate, changedDescription));
		}

		[Test]
		public void ChangeBooking_AccountClosed_InvalidOperationException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var changedPayment = Payment.FromDecimal(200.00m, "EUR");
			var changedBookingDate = BookingDate.FromString("2020-09-01");
			var changedDescription = Description.FromString("My payment changed");

			account.Close();

			//Act & Assert
			Assert.Throws<InvalidOperationException>(() => account.ChangeBooking(bookingId, changedPayment, changedBookingDate, changedDescription));
		}

		[Test]
		public void ChangeBooking_BookingWithDifferentCurrency_PaymentNotSameCurrencyAsAccountException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var changedPayment = Payment.FromDecimal(200.00m, "USD");
			var changedBookingDate = BookingDate.FromString("2020-09-01");
			var changedDescription = Description.FromString("My payment changed");

			//Act & Assert
			Assert.Throws<PaymentNotSameCurrencyAsAccountException>(() => account.ChangeBooking(bookingId, changedPayment, changedBookingDate, changedDescription));
		}

		[Test]
		public void ChangeBooking_BookingDoesNotExist_InvalidOperationException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var nonExistingBookingId = BookingId.FromGuid(Guid.Parse("58bead84-e4d6-481b-9c7a-b52a36a98c3f"));
			var changedPayment = Payment.FromDecimal(200.00m, "EUR");
			var changedBookingDate = BookingDate.FromString("2020-09-01");
			var changedDescription = Description.FromString("My payment changed");

			//Act & Assert
			Assert.Throws<InvalidOperationException>(() => account.ChangeBooking(nonExistingBookingId, changedPayment, changedBookingDate, changedDescription));
		}

		#endregion ChangeBooking

		#region DeleteBooking

		[Test]
		public void DeleteBooking_ExistingBookingId_BookingRemoved()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			//Act
			account.DeleteBooking(bookingId);

			//Assert
			Assert.AreEqual(0, account.Bookings.Count);
		}

		[Test]
		public void DeleteBooking_AccountClosed_InvalidOperationException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			account.Close();

			//Act & assert
			Assert.Throws<InvalidOperationException>(() => account.DeleteBooking(bookingId));
		}

		[Test]
		public void DeleteBooking_NonExistingBookingId_InvalidOperationException()
		{
			//Arrange
			var bookingIdGuid = Guid.Parse("b9c18f4d-8f11-408d-be7c-c7f022abefb2");
			var bookingId = BookingId.FromGuid(bookingIdGuid);
			var payment = Payment.FromDecimal(100.00m, "EUR");
			var bookingDate = BookingDate.FromString("2020-09-26");
			var description = Description.FromString("My payment");

			var accountIdGuid = Guid.Parse("9ccf4aa8-cd1e-4044-a183-464c8a8730ec");
			var account = AccountRoot.Create(
					AccountId.FromGuid(accountIdGuid),
					UserId.FromGuid(Guid.Parse("13f172e2-7189-4506-b232-894bafcd4449")),
					Period.FromMonth(2020, 9),
					CurrencyCode.FromString("EUR"));

			account.BookPayment(bookingId, payment, bookingDate, description);

			var nonExistingBookingId = BookingId.FromGuid(Guid.Parse("2a5be36a-618e-48d1-a0e7-a184f2081a72"));

			//Act & Assert
			Assert.Throws<InvalidOperationException>(() => account.DeleteBooking(nonExistingBookingId));
		}

		#endregion DeleteBooking

		#region Close

		[Test]
		public void Close_StateClosedAmountSumOfBookings()
		{
			//Arrange
			var idGuid = Guid.Parse("051f8160-ce43-4ac0-b8c2-09707c2bcda3");
			var id = AccountId.FromGuid(idGuid);
			var ownerGuid = Guid.Parse("4bc0c0e9-7181-45e4-934d-e91c0e7bbd75");
			var ownerId = UserId.FromGuid(ownerGuid);
			var period = Period.FromMonth(2020, 9);
			var currency = CurrencyCode.FromString("EUR");

			var account = AccountRoot.Create(id, ownerId, period, currency);

			account.BookPayment(
				BookingId.FromGuid(Guid.Parse("9d8d8a72-59a0-4a1b-8b86-94960df74586")),
				Payment.FromDecimal(10.00m, "EUR"),
				BookingDate.FromString("2020-09-01"),
				Description.FromString("Booking1"));

			account.BookPayment(
				BookingId.FromGuid(Guid.Parse("cbf7333e-3a7a-4bf9-ac85-4bdb22f7afb0")),
				Payment.FromDecimal(5.10m, "EUR"),
				BookingDate.FromString("2020-09-02"),
				Description.FromString("Booking2"));

			account.BookPayment(
				BookingId.FromGuid(Guid.Parse("07a94bd6-ae33-4765-9d1d-5257297e7ed0")),
				Payment.FromDecimal(8.43m, "EUR"),
				BookingDate.FromString("2020-09-03"),
				Description.FromString("Booking3"));

			//Act
			account.Close();

			//Assert
			Assert.AreEqual(AccountState.AccountStateEnum.Closed, account.State.Value);
			Assert.AreEqual(23.53m, account.TotalAmountPaid.Amount);
			Assert.IsTrue(account.GetChanges().Last() is V1.AccountClosed);
			var @event = account.GetChanges().Last() as V1.AccountClosed;
			Assert.AreEqual(23.53m, @event.TotalPaidAmount);
			Assert.AreEqual("EUR", @event.CurrencyCode);
			Assert.AreEqual(idGuid, @event.Id);
		}

		[Test]
		public void Close_AccountAlreadyClosed_InvalidOperationException()
		{
			//Arrange
			var idGuid = Guid.Parse("051f8160-ce43-4ac0-b8c2-09707c2bcda3");
			var id = AccountId.FromGuid(idGuid);
			var ownerGuid = Guid.Parse("4bc0c0e9-7181-45e4-934d-e91c0e7bbd75");
			var ownerId = UserId.FromGuid(ownerGuid);
			var period = Period.FromMonth(2020, 9);
			var currency = CurrencyCode.FromString("EUR");

			var account = AccountRoot.Create(id, ownerId, period, currency);

			account.Close();

			//Act & Assert
			Assert.Throws<InvalidOperationException>(() => account.Close());
		}

		#endregion Close
	}
}
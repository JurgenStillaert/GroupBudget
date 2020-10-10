using GroupBudget.SharedKernel;
using NUnit.Framework;
using System;

namespace GroupBudget.Account.Domain.Tests
{
	internal class ValueObjectTests
	{
		#region AccountId

		[Test]
		public void AccountId_ValidGuid_Created()
		{
			//Arrange
			var guid = Guid.Parse("dca2b44e-6ea7-4812-8c2f-ca772512ca01");

			//Act
			var accountId = AccountId.FromGuid(guid);

			//Assert
			Assert.AreEqual(guid, accountId.Value);
		}

		[Test]
		public void AccountId_DefaultGuid_ArgumentNullException()
		{
			//Arrange
			var guid = Guid.Empty;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => AccountId.FromGuid(guid));
		}

		#endregion AccountId

		#region AccountState

		[Test]
		public void AccountState_CreateOpen_Created()
		{
			//Act
			var accountState = AccountState.CreateOpen();

			//Assert
			Assert.AreEqual(AccountState.AccountStateEnum.Open, accountState.Value);
		}

		[Test]
		public void AccountState_CreateClosed_Created()
		{
			//Act
			var accountState = AccountState.CreateClosed();

			//Assert
			Assert.AreEqual(AccountState.AccountStateEnum.Closed, accountState.Value);
		}

		#endregion AccountState

		#region BookingDate
		[Test]
		public void BookingDate_FromDate_ValidDate_Created()
		{
			//Arrange
			var dte = DateTime.Parse("2020-09-26");

			//Act
			var bookingDate = BookingDate.FromDate(dte);

			//Assert
			Assert.AreEqual(dte, bookingDate.Value);
		}

		[Test]
		public void BookingDate_FromDate_DefaultDate_ArgumentNullException()
		{
			//Arrange
			var dte = DateTime.MinValue;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => BookingDate.FromDate(dte));
		}

		[Test]
		public void BookingDate_FromString_ValidDate_Created()
		{
			//Arrange
			var dte = DateTime.Parse("2020-09-26");

			//Act
			var bookingDate = BookingDate.FromString("2020-09-26");

			//Assert
			Assert.AreEqual(dte, bookingDate.Value);
		}

		[Test]
		public void BookingDate_FromString_InvalidDate_ArgumentNullException()
		{
			//Arrange
			const string dte = "hello";

			//Act & Assert
			Assert.Throws<FormatException>(() => BookingDate.FromString(dte));
		}
		#endregion

		#region BookingId

		[Test]
		public void BookingId_ValidGuid_Created()
		{
			//Arrange
			var guid = Guid.Parse("dca2b44e-6ea7-4812-8c2f-ca772512ca01");

			//Act
			var bookingId = BookingId.FromGuid(guid);

			//Assert
			Assert.AreEqual(guid, bookingId.Value);
		}

		[Test]
		public void BookingId_DefaultGuid_ArgumentNullException()
		{
			//Arrange
			var guid = Guid.Empty;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => BookingId.FromGuid(guid));
		}

		#endregion AccountId

		#region CurrencyCode
		[Test]
		public void CurrencyCode_FromString_ValidCurrencyCode_Created()
		{
			//Arrange
			const string currencyString = "EUR";

			//Act
			var currencyCode = CurrencyCode.FromString(currencyString);

			//Assert
			Assert.AreEqual(currencyString, currencyCode.Value);
		}

		[Test]
		public void CurrencyCode_FromString_LowercaseCurrencyCode_CreatedUppercase()
		{
			//Arrange
			const string currencyString = "eur";

			//Act
			var currencyCode = CurrencyCode.FromString(currencyString);

			//Assert
			Assert.AreEqual("EUR", currencyCode.Value);
		}

		[Test]
		public void CurrencyCode_FromString_EmptyString_ArgumentNullException()
		{
			//Arrange
			var currencyString = string.Empty;

			//Act & assert
			Assert.Throws<ArgumentNullException>(() => CurrencyCode.FromString(currencyString));
		}

		[Test]
		public void CurrencyCode_FromString_null_ArgumentNullException()
		{
			//Act & assert
			Assert.Throws<ArgumentNullException>(() => CurrencyCode.FromString(null));
		}
		#endregion

		#region Description
		[Test]
		public void Description_FromString_ValidString_Created()
		{
			//Arrange
			const string descString = "This is my description";

			//Act
			var description = Description.FromString(descString);

			//Assert
			Assert.AreEqual(descString, description.Value);
		}

		[Test]
		public void Description_FromString_TooShortString_ArgumentException()
		{
			//Arrange
			const string descString = "a";

			//Act & assert
			Assert.Throws<ArgumentException>(() => Description.FromString(descString));
		}

		[Test]
		public void Description_FromString_TooLongString_ArgumentException()
		{
			//Arrange
			const string descString = "Description must have a maximum of 30 charachters";

			//Act & assert
			Assert.Throws<ArgumentException>(() => Description.FromString(descString));
		}

		[Test]
		public void Description_FromString_NullString_NullReferenceException()
		{
			//Arrange
			const string descString = null;

			//Act & assert
			Assert.Throws<NullReferenceException>(() => Description.FromString(descString));
		}
		#endregion

		#region UserId

		[Test]
		public void UserId_ValidGuid_Created()
		{
			//Arrange
			var guid = Guid.Parse("dca2b44e-6ea7-4812-8c2f-ca772512ca01");

			//Act
			var userId = UserId.FromGuid(guid);

			//Assert
			Assert.AreEqual(guid, userId.Value);
		}

		[Test]
		public void UserId_DefaultGuid_ArgumentNullException()
		{
			//Arrange
			var guid = Guid.Empty;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => UserId.FromGuid(guid));
		}

		#endregion AccountId

		#region Payment
		[Test]
		public void Payment_FromDecimal_PositiveAmountAndValidCurrency_Created()
		{
			//Arrange


			//Act

			//Assert

		}
		#endregion
	}
}
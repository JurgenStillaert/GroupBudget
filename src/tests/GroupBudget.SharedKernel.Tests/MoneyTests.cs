using NUnit.Framework;
using System;

namespace GroupBudget.SharedKernel.Tests
{
	public class MoneyTests
	{
		#region FromDecimal
		[Test]
		public void FromDecimal_ValidAmountAndCurrencyCode_Created()
		{
			//Arrange
			const decimal amount = 10.00M;
			const string currency = "EUR";

			//Act
			var money = Money.FromDecimal(amount, currency);

			//Assert
			Assert.IsNotNull(money);
			Assert.AreEqual(amount, money.Amount);
			Assert.AreEqual(currency, money.CurrencyCode);
		}

		[Test]
		public void FromDecimal_ValidAmountAndEmptyCurrencyCode_ArgumentNullException()
		{
			//Arrange
			const decimal amount = 10.00M;
			var currency = string.Empty;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Money.FromDecimal(amount, currency));
		}

		[Test]
		public void FromDecimal_ValidAmountAndNullCurrencyCode_ArgumentNullException()
		{
			//Arrange
			const decimal amount = 10.00M;
			const string currency = null;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Money.FromDecimal(amount, currency));
		}
		#endregion

		#region FromString
		[Test]
		public void FromString_ValidAmountAndCurrencyCode_Created()
		{
			//Arrange
			const string amount = "10.00";
			const string currency = "EUR";

			//Act
			var money = Money.FromString(amount, currency);

			//Assert
			Assert.IsNotNull(money);
			Assert.AreEqual(10m, money.Amount);
			Assert.AreEqual(currency, money.CurrencyCode);
		}

		[Test]
		public void FromString_ValidAmountAndEmptyCurrencyCode_ArgumentNullException()
		{
			//Arrange
			const string amount = "10.00";
			var currency = string.Empty;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Money.FromString(amount, currency));
		}

		[Test]
		public void FromString_ValidAmountAndNullCurrencyCode_ArgumentNullException()
		{
			//Arrange
			const string amount = "10.00";
			const string currency = null;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Money.FromString(amount, currency));
		}

		[Test]
		public void FromString_EmptyAmountAndValidCurrencyCode_FormatException()
		{
			//Arrange
			var amount = string.Empty;
			const string currency = "EUR";

			//Act & Assert
			Assert.Throws<FormatException>(() => Money.FromString(amount, currency));
		}

		[Test]
		public void FromString_NullAmountAndValidCurrencyCode_ArgumentNullException()
		{
			//Arrange
			const string amount = null;
			const string currency = "EUR";

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Money.FromString(amount, currency));
		}
		#endregion

		#region Add
		[Test]
		public void Add_TwoPositiveMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "EUR");

			//Act
			var totalMoney = money1.Add(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Add_PositiveMoneyAndNegativeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1.Add(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Add_TwoNegativeeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(-10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1.Add(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(-15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Add_TwoMoneyWithDifferentCurrency_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "USD");

			//Act & Assert
			Assert.Throws<CurrencyMismatchException>(() => money1.Add(money2));
		}

		#endregion

		#region Subtract
		[Test]
		public void Subtract_TwoPositiveMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "EUR");

			//Act
			var totalMoney = money1.Subtract(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Subtract_PositiveMoneyAndNegativeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1.Subtract(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Subtract_TwoNegativeeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(-10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1.Subtract(money2);

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(-5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void Subtract_TwoMoneyWithDifferentCurrency_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "USD");

			//Act & Assert
			Assert.Throws<CurrencyMismatchException>(() => money1.Subtract(money2));
		}

		#endregion

		#region PlusOperator
		[Test]
		public void PlusOperator_TwoPositiveMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "EUR");

			//Act
			var totalMoney = money1 + money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void PlusOperator_PositiveMoneyAndNegativeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1 + money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void PlusOperator_TwoNegativeeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(-10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1 + money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(-15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void PlusOperator_TwoMoneyWithDifferentCurrency_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "USD");

			//Act & Assert
			Assert.Throws<CurrencyMismatchException>(() => { var totalMoney = money1 + money2; });
		}

		#endregion

		#region NegativeOperator
		[Test]
		public void NegativeOperator_TwoPositiveMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "EUR");

			//Act
			var totalMoney = money1 - money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void NegativeOperator_PositiveMoneyAndNegativeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1 - money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(15.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void NegativeOperator_TwoNegativeeMoney_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(-10.00M, "EUR");
			var money2 = Money.FromDecimal(-5.00M, "EUR");

			//Act
			var totalMoney = money1 - money2;

			//Assert
			Assert.IsNotNull(totalMoney);
			Assert.AreEqual(-5.00M, totalMoney.Amount);
			Assert.AreEqual("EUR", totalMoney.CurrencyCode);
		}

		[Test]
		public void NegativeOperator_TwoMoneyWithDifferentCurrency_NewMoneyCreated()
		{
			//Arrange
			var money1 = Money.FromDecimal(10.00M, "EUR");
			var money2 = Money.FromDecimal(5.00M, "USD");

			//Act & Assert
			Assert.Throws<CurrencyMismatchException>(() => { var totalMoney = money1 - money2; });
		}
		#endregion

		#region ToString
		[Test]
		public void ToString_ValidMoney_StringRepresentation()
		{
			//Arrange
			var money = Money.FromDecimal(10.00M, "EUR");

			//Act
			var strMoney = money.ToString();

			//Assert
			Assert.AreEqual("10.00 EUR", strMoney);
		}
		#endregion
	}
}
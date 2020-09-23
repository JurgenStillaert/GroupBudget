using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class Money : Value<Money>
	{
		public decimal Amount { get; }
		public string CurrencyCode { get; }

		protected Money(decimal amount, string currencyCode)
		{
			if (string.IsNullOrEmpty(currencyCode))
			{
				throw new ArgumentNullException(nameof(currencyCode), "Currency code cannot be empty");
			}

			Amount = amount;
			CurrencyCode = currencyCode.ToUpper();
		}

		public static Money FromDecimal(decimal amount, string currencyCode)
			=> new Money(amount, currencyCode);

		public static Money FromString(string amount, string currencyCode)
			=> new Money(decimal.Parse(amount), currencyCode);

		public Money Add(Money amountToAdd)
		{
			if (CurrencyCode != amountToAdd.CurrencyCode)
			{
				throw new CurrencyMismatchException("Cannot add amounts from different currencies");
			}

			return new Money(Amount + amountToAdd.Amount, CurrencyCode);
		}

		public Money Subtract(Money amountToSubtract)
		{
			if (CurrencyCode != amountToSubtract.CurrencyCode)
			{
				throw new CurrencyMismatchException("Cannot subtract amounts from different currencies");
			}

			return new Money(Amount - amountToSubtract.Amount, CurrencyCode);
		}

		public static Money operator +(Money amount1, Money amount2) => amount1.Add(amount2);

		public static Money operator -(Money amount1, Money amount2) => amount1.Subtract(amount2);

		public override string ToString()
			=> $"{CurrencyCode} {Amount.ToString("F")}";
	}

	public class CurrencyMismatchException : Exception
	{
		public CurrencyMismatchException(string message) : base(message)
		{
		}
	}
}
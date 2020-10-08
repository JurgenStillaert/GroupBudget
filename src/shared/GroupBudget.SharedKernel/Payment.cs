using System;

namespace GroupBudget.SharedKernel
{
	public class Payment : Money
	{
		protected Payment(decimal amount, string currencyCode)
			: base(amount, currencyCode)
		{
			if (amount <= 0)
				throw new ArgumentException("Payment cannot be negative or zero", nameof(amount));
		}

		public new static Payment FromDecimal(decimal amount, string currencyCode)
			=> new Payment(amount, currencyCode);

		public new static Payment FromString(string amount, string currencyCode)
			=> new Payment(decimal.Parse(amount), currencyCode);
	}
}
using System;

namespace GroupBudget.Account.Domain
{
	public sealed class Payment : Money
	{
		protected Payment(decimal amount, string currencyCode)
			: base(amount, currencyCode)
		{
			if (amount < 0)
				throw new ArgumentException("Payment cannot be negative", nameof(amount));
		}

		public new static Payment FromDecimal(decimal amount, string currencyCode)
			=> new Payment(amount, currencyCode);
	}
}
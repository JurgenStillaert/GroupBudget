using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class CurrencyCode : Value<CurrencyCode>
	{
		public string Value { get; }

		public CurrencyCode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("Currency code cannot be empty", nameof(value));
			}

			Value = value.ToUpper();
		}

		public static CurrencyCode FromString(string value) => new CurrencyCode(value);
	}
}
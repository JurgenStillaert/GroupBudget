using System;

namespace GroupBudget.Account.Domain
{
	public partial class Exceptions
	{
		[Serializable]
		public class PaymentNotSameCurrencyAsAccountException : Exception
		{
			public PaymentNotSameCurrencyAsAccountException()
			{
			}

			public PaymentNotSameCurrencyAsAccountException(string message) : base(message)
			{
			}

			public PaymentNotSameCurrencyAsAccountException(string message, Exception inner) : base(message, inner)
			{
			}

			protected PaymentNotSameCurrencyAsAccountException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}
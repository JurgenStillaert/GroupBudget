using NUnit.Framework;
using System;

namespace GroupBudget.SharedKernel.Tests
{
	public class PaymentTests
	{
		[Test]
		public void FromDecimal_NegativePayment_ArgumentException()
		{
			//Act & Assert
			Assert.Throws<ArgumentException>(() => Payment.FromDecimal(-1M, "EUR"));
		}

		[Test]
		public void FromDecimal_ZeroPayment_ArgumentException()
		{
			//Act & Assert
			Assert.Throws<ArgumentException>(() => Payment.FromDecimal(0, "EUR"));
		}
	}
}
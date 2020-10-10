using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class PaymentSettlementId : Value<PaymentSettlementId>
	{
		public Guid Value { get; }

		protected PaymentSettlementId(Guid value)
		{
			if (value == default)
			{
				throw new ArgumentNullException(nameof(value), "The paymentsettle id cannot be empty");
			}

			Value = value;
		}

		public static PaymentSettlementId FromGuid(Guid value) => new PaymentSettlementId(value);
		public static PaymentSettlementId FromString(string ownerId) => new PaymentSettlementId(Guid.Parse(ownerId));


		public static implicit operator Guid(PaymentSettlementId self) => self.Value;
	}
}
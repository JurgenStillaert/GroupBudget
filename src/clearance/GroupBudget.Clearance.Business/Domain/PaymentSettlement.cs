using Codefondo.DDD.Kernel;
using GroupBudget.SharedKernel;
using static GroupBudget.Clearance.Messages.Events;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class PaymentSettlement : Entity<PaymentSettlementId>
	{
		public UserId Payer { get; private set; }
		public UserId Receiver { get; private set; }
		public Payment Amount { get; private set; }

		public PaymentSettlement(V1.ClearanceFinalized @event)
		{
			Payer = UserId.FromGuid(@event.Payer);
			Receiver = UserId.FromGuid(@event.Receiver);
			Amount = Payment.FromDecimal(@event.Amount, @event.CurrencyCode);
		}

		protected override void EnsureValidation()
		{
			//
		}
	}
}
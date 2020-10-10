using Codefondo.DDD.Kernel;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class ClearanceState : Value<ClearanceState>
	{
		public ClearanceStateEnum Value { get; }

		public ClearanceState(ClearanceStateEnum value)
		{
			Value = value;
		}

		public static ClearanceState CreateOpen() => new ClearanceState(ClearanceStateEnum.Open);

		public static ClearanceState CreateClosed() => new ClearanceState(ClearanceStateEnum.Closed);

		public enum ClearanceStateEnum
		{
			Open,
			Closed
		}
	}
}
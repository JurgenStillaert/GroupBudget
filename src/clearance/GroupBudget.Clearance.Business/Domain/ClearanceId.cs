using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class ClearanceId : AggregateId<ClearanceRoot>
	{
		public ClearanceId(Guid value) : base(value)
		{
		}

		public static ClearanceId FromGuid(Guid value) => new ClearanceId(value);

		public static ClearanceId FromString(string id) => new ClearanceId(Guid.Parse(id));
	}
}
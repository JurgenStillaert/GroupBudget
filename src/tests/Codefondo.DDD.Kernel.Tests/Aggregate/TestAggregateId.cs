using System;

namespace Codefondo.DDD.Kernel.Tests.Aggregate
{
	public class TestAggregateId : AggregateId<TestAggregate>
	{
		public TestAggregateId(Guid value) : base(value)
		{
		}

		public static TestAggregateId FromGuid(Guid value) => new TestAggregateId(value);
	}
}
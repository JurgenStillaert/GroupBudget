using System;

namespace Codefondo.DDD.Kernel.Tests.Aggregate
{
	public class TestEntityId : Value<TestEntityId>
	{
#pragma warning disable CC0057 // Unused parameters

		public TestEntityId(Guid value) => Value = value;

#pragma warning restore CC0057 // Unused parameters

		public Guid Value { get; }

		public static TestEntityId FromGuid(Guid value) => new TestEntityId(value);
	}
}
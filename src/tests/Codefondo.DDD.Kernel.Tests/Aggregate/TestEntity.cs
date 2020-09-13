using Codefondo.DDD.Kernel.Tests.Aggregate.DomainEvents;
using Codefondo.DDD.Kernel.Tests.Values;

namespace Codefondo.DDD.Kernel.Tests.Aggregate
{
	public class TestEntity : Entity<TestEntityId>
	{
		public StringValue MyString { get; private set; }

		internal static TestEntity Create(TestEntityAddedToTestAggregateEvent @event)
		{
			var te = new TestEntity();

			te.Apply(@event);

			return te;
		}

		public void CommandNotImplementedEventHandler()
		{
			Apply(new EventNotImplemented());
		}

		protected override void EnsureValidation()
		{
			var valid = !string.IsNullOrEmpty(MyString) && MyString.Value.Length > 3;

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "");
			}
		}

		private void Handle(TestEntityAddedToTestAggregateEvent @event)
		{
			Id = TestEntityId.FromGuid(@event.Id);
			MyString = StringValue.FromString(@event.MyString);
		}
	}
}
using Codefondo.DDD.Kernel.Tests.Aggregate.DomainEvents;
using Codefondo.DDD.Kernel.Tests.Values;

namespace Codefondo.DDD.Kernel.Tests.Aggregate
{
	public class TestAggregate : AggregateRoot
	{
		public StringValue MyString { get; private set; }
		public BoolValue MyBool { get; private set; }

		public static TestAggregate Create(
			TestAggregateId id,
			StringValue stringValue,
			BoolValue boolValue)
		{
			var ta = new TestAggregate();

			ta.Apply(new CreatedEvent(id, stringValue, boolValue));

			return ta;
		}

		public void CommandNotImplementedEventHandler()
		{
			Apply(new EventNotImplemented());
		}

		public void UpdateString(StringValue str)
		{
			Apply(new StringUpdatedEvent(Id, str));
		}

		protected override void EnsureValidation()
		{
			var valid =
				!string.IsNullOrEmpty(MyString) && MyString.Value.Length > 6
				&&
				MyBool == true;

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "");
			}
		}

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CC0068 // Unused Method
		private void Handle(CreatedEvent @event)
		{
			Id = @event.Id;
			MyString = StringValue.FromString(@event.MyString);
			MyBool = BoolValue.FromBool(@event.MyBool);
		}

		private void Handle(StringUpdatedEvent @event)
		{
			MyString = StringValue.FromString(@event.MyString);
		}
#pragma warning restore CC0068 // Unused Method
#pragma warning restore IDE0051 // Remove unused private members
	}
}
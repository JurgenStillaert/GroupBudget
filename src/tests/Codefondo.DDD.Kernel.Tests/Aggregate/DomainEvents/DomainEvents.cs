using System;

namespace Codefondo.DDD.Kernel.Tests.Aggregate.DomainEvents
{
	public sealed class CreatedEvent : IDomainEvent
	{
		public Guid Id { get; }
		public string MyString { get; }
		public bool MyBool { get; }

		public CreatedEvent(Guid id, string myString, bool myBool)
		{
			Id = id;
			MyString = myString;
			MyBool = myBool;
		}
	}

	public sealed class StringUpdatedEvent : IDomainEvent
	{
		public Guid Id { get; }
		public string MyString { get; }

		public StringUpdatedEvent(Guid id, string myString)
		{
			Id = id;
			MyString = myString;
		}
	}

	public sealed class EventNotImplemented : IDomainEvent
	{ }
}
using Codefondo.DDD.Kernel.Tests.Aggregate;
using Codefondo.DDD.Kernel.Tests.Aggregate.DomainEvents;
using Codefondo.DDD.Kernel.Tests.Values;
using NUnit.Framework;
using System;
using static Codefondo.DDD.Kernel.DomainExceptions;

namespace Codefondo.DDD.Kernel.Tests
{
	public class AggregateTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Create_ValidData_ReturnsAggregate()
		{
			//Arrange
			var guid = Guid.Parse("5ffdfca6-2cb6-4a22-8943-339a65298a2b");

			//Act
			var ta = TestAggregate.Create(TestAggregateId.FromGuid(guid), StringValue.FromString("testtest"), BoolValue.FromBool(true));

			//Assert
			Assert.IsNotNull(ta);
			Assert.AreEqual(guid, ta.Id);
			Assert.AreEqual("testtest", ta.MyString.Value);
			Assert.AreEqual(true, ta.MyBool.Value);
		}

		[Test]
		public void CommandNotImplementedEventHandler_ThrowsMissingMethodException()
		{
			//Arrange
			var guid = Guid.Parse("5ffdfca6-2cb6-4a22-8943-339a65298a2b");
			var ta = TestAggregate.Create(TestAggregateId.FromGuid(guid), StringValue.FromString("testtest"), BoolValue.FromBool(true));

			//Act and Assert
			Assert.Throws<MissingMethodException>(() => ta.CommandNotImplementedEventHandler());
		}

		[Test]
		public void GetChanges_CommandsExecuted_EventsReturned()
		{
			//Arrange
			var guid = Guid.Parse("5ffdfca6-2cb6-4a22-8943-339a65298a2b");
			var ta = TestAggregate.Create(TestAggregateId.FromGuid(guid), StringValue.FromString("testtest"), BoolValue.FromBool(true));
			ta.UpdateString(StringValue.FromString("Hello world"));

			//Act
			var changes = ta.GetChanges();

			//Arrange
			Assert.AreEqual(2, changes.Count);
			Assert.IsTrue(changes[0] is CreatedEvent);
			Assert.IsTrue(changes[1] is StringUpdatedEvent);
		}

		[Test]
		public void ClearChanges_NoEventsReturned()
		{
			//Arrange
			var guid = Guid.Parse("5ffdfca6-2cb6-4a22-8943-339a65298a2b");
			var ta = TestAggregate.Create(TestAggregateId.FromGuid(guid), StringValue.FromString("testtest"), BoolValue.FromBool(true));
			ta.UpdateString(StringValue.FromString("Hello world"));

			//Act
			ta.ClearChanges();
			var changes = ta.GetChanges();

			//Arrange
			Assert.AreEqual(0, changes.Count);
		}

		[Test]
		public void EnsureValidation_Called()
		{
			//Arrange
			var guid = Guid.Parse("5ffdfca6-2cb6-4a22-8943-339a65298a2b");

			//Act and arrange
			Assert.Throws<InvalidEntityState>(() => TestAggregate.Create(TestAggregateId.FromGuid(guid), StringValue.FromString("short"), BoolValue.FromBool(true)));
		}
	}
}
using Codefondo.DDD.Kernel;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codefondo.UseCase.Kernel
{
	public class Repository<TAggregate> : IRepository<TAggregate>
		where TAggregate : AggregateRoot
	{
		private readonly IEventStoreConnection connection;

		public Repository(IEventStoreConnection connection)
		{
			this.connection = connection;
		}

		public async Task<AggregateRoot> Load(Guid aggregateId)
		{
			var streamName = GetStreamName(aggregateId);

			var eventSlice = await connection.ReadStreamEventsForwardAsync(streamName, 0, 4096, false);

			var aggregate = Activator.CreateInstance(typeof(TAggregate), true) as AggregateRoot;
			aggregate.Replay(eventSlice.Events.Select(x => Deserialize(x.Event)).ToList());

			return aggregate;
		}

		public async Task Save(TAggregate aggregateRoot)
		{
			var streamName = GetStreamName(aggregateRoot.Id);

			var eventData = aggregateRoot.GetChanges().Select(x =>
				new EventData(
					Guid.NewGuid(),
					streamName,
					true,
					Encoding.UTF8.GetBytes(SerializeEvent(x)),
					Encoding.UTF8.GetBytes(SerializeMetaData(x)))
			);

			await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
		}

		private static string SerializeMetaData(IDomainEvent @event)
		{
			var metaData = new EventMetaData
			{
				EventType = @event.GetType().AssemblyQualifiedName
			};

			return JsonConvert.SerializeObject(metaData);
		}

		private class EventMetaData
		{
			public string EventType { get; set; }
		}
		private static string SerializeEvent(IDomainEvent @event)
		{
			return JsonConvert.SerializeObject(@event);
		}

		private static IDomainEvent Deserialize(RecordedEvent resolvedEvent)
		{
			var jsonData = Encoding.UTF8.GetString(resolvedEvent.Data);
			var metaData = JsonConvert.DeserializeObject<EventMetaData>(Encoding.UTF8.GetString(resolvedEvent.Metadata));

			var data = JsonConvert.DeserializeObject(jsonData, Type.GetType(metaData.EventType)) as IDomainEvent;
			return data;
		}

		private static string GetStreamName(Guid aggregateId)
		{
			return $"{typeof(TAggregate).Name}-{aggregateId}";
		}
	}
}
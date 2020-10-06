using Codefondo.DDD.Kernel;
using EventStore.ClientAPI;
using GroupBudget.Account.Domain;
using GroupBudget.Account.UseCases;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget.Account.Persistence
{
	public class AccountRepository : IAccountRepository
	{
		private readonly IEventStoreConnection connection;

		public AccountRepository(IEventStoreConnection connection)
		{
			this.connection = connection;
		}

		public async Task<AggregateRoot> Load(Guid aggregateId)
		{
			var streamName = $"account-{aggregateId}";

			var eventSlice = await connection.ReadStreamEventsForwardAsync(streamName, 0, 4096, false);

			var aggregate = Activator.CreateInstance(typeof(AccountRoot), true) as AggregateRoot;
			aggregate.Replay(eventSlice.Events.Select(x => Deserialize(x.Event)).ToList());

			return aggregate;
		}

		public async Task Save(AccountRoot aggregateRoot)
		{
			var streamName = $"account-{aggregateRoot.Id}";

			var eventData = aggregateRoot.GetChanges().Select(x =>
				new EventData(
					Guid.NewGuid(),
					streamName,
					true,
					Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })),
					null)
			);

			await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
		}

		public class EventMetaData
		{
			public string EventType { get; set; }
		}

		public static IDomainEvent Deserialize(RecordedEvent resolvedEvent)
		{
			var jsonData = Encoding.UTF8.GetString(resolvedEvent.Data);
			var data = JsonConvert.DeserializeObject(jsonData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }) as IDomainEvent;
			return data;
		}
	}
}
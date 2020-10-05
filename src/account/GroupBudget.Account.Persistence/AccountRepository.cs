using Codefondo.DDD.Kernel;
using EventStore.ClientAPI;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Persistence.EventStore;
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
		//public readonly IEventStoreConnection connection;
		public AccountRepository()
		{
		}

		public async Task<AggregateRoot> Load(Guid aggregateId)
		{
			var streamName = $"account-{aggregateId}";

			var connection = EventstoreConnectionHelper.CreateConnection();

			var events = await connection.ReadEventAsync(streamName, StreamPosition.Start, false);

			return null;
		}

		public async Task Save(AccountRoot aggregateRoot)
		{
			var streamName = $"account-{aggregateRoot.Id}";

			var connection = EventstoreConnectionHelper.CreateConnection();

			var eventData = aggregateRoot.GetChanges().Select(x =>
				new EventData(
					Guid.NewGuid(),
					streamName,
					true,
					Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x)),
					Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new EventMetaData { EventType = x.GetType().FullName })))
			);

			await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
		}

		public class EventMetaData
		{
			public string EventType { get; set; }
		}
	}
}
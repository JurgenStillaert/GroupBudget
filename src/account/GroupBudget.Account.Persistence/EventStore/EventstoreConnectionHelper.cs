using EventStore.ClientAPI;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GroupBudget.Account.Persistence.EventStore
{
	public class EventstoreConnectionHelper
	{
		public static IEventStoreConnection CreateConnection()
		{
			var conn = EventStoreConnection.Create(ConnectionSettings.Create().KeepReconnecting().DisableServerCertificateValidation().DisableTls(), new System.Uri("tcp://admin:changeit@eventstore.db:1113"));
			conn.ConnectAsync().Wait();
			return conn;
		}
	}
}
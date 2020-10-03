namespace GroupBudget.Account.Persistence.MongoDb
{
	public class AccountDatabaseSettings : IAccountDatabaseSettings
	{
		public string AccountDtoCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}

	public interface IAccountDatabaseSettings
	{
		string AccountDtoCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}
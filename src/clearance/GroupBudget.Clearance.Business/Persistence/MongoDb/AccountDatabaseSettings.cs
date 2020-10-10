namespace GroupBudget.Clearance.Business.Persistence.MongoDb
{
	public class ClearanceDatabaseSettings : IClearanceDatabaseSettings
	{
		public string ClearanceDtoCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}

	public interface IClearanceDatabaseSettings
	{
		string ClearanceDtoCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}
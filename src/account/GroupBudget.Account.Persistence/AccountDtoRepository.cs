using GroupBudget.Account.Dtos;
using GroupBudget.Account.Persistence.MongoDb;
using GroupBudget.Account.UseCases.EventHandlers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.Account.Persistence
{
	public class AccountDtoRepository : IAccountDtoRepository
	{
		private readonly IMongoCollection<AccountDto> accounts;

		public AccountDtoRepository(IAccountDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			accounts = database.GetCollection<AccountDto>(settings.AccountDtoCollectionName);
		}

		public async Task Create(AccountDto accountDto)
		{
			accounts.InsertOne(accountDto);
			await Task.CompletedTask;
		}

		public async Task<List<AccountDto>> GetAccountDtosByUserId(Guid ownerId)
		{
			var result = await accounts.FindAsync(x => x.OwnerId == ownerId.ToString());
			return await result.ToListAsync();
		}
	}
}
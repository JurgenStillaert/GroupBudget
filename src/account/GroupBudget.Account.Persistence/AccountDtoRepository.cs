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

		public async Task Update(AccountDto accountDto)
		{
			var filter = Builders<AccountDto>.Filter.Eq(x => x.Id, accountDto.Id);

			await accounts.ReplaceOneAsync(filter, accountDto);
		}

		public async Task<List<AccountDto>> GetAccountDtosByUserId(Guid ownerId)
		{
			var result = await accounts.FindAsync(x => x.OwnerId == ownerId.ToString());
			return await result.ToListAsync();
		}

		public async Task<AccountDto> GetAccountByAccountId(Guid accountId)
		{
			var result = await accounts.FindAsync(x => x.Id == accountId.ToString());
			return await result.SingleAsync();
		}
	}
}
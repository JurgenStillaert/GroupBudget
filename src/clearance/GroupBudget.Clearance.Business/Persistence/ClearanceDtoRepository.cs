using GroupBudget.Clearance.Business.Domain;
using GroupBudget.Clearance.Business.Persistence.MongoDb;
using GroupBudget.Clearance.Messages.Dtos;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.Persistence
{
	public class ClearanceDtoRepository : IClearanceDtoRepository
	{
		private readonly IMongoCollection<ClearanceDto> clearances;

		public ClearanceDtoRepository(IClearanceDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			clearances = database.GetCollection<ClearanceDto>(settings.ClearanceDtoCollectionName);
		}

		public async Task Create(ClearanceDto clearanceDto)
		{
			clearances.InsertOne(clearanceDto);
			await Task.CompletedTask;
		}

		public async Task Update(ClearanceDto clearanceDto)
		{
			var filter = Builders<ClearanceDto>.Filter.Eq(x => x.Id, clearanceDto.Id);

			await clearances.ReplaceOneAsync(filter, clearanceDto);
		}

		public async Task<Guid?> GetClearanceIdByAccountId(Guid accountId)
		{
			var fb = Builders<ClearanceDto>.Filter;
			var filter = fb.ElemMatch(x => x.UserAccounts, ua => ua.AccountId == accountId);

			var findCursor = await clearances.FindAsync(filter);
			var clearance = await findCursor.FirstOrDefaultAsync();

			if (clearance == null)
			{
				return null;
			}
			else
			{
				return Guid.Parse(clearance.Id);
			}
		}

		public async Task<ClearanceDto> GetClearanceIdForUserOnPeriod(Guid userId, DateTime startDate, DateTime endDate)
		{
			var fb = Builders<ClearanceDto>.Filter;
			var filter = fb.Eq(x => x.StartDate, startDate)
							& fb.Eq(x => x.EndDate, endDate)
							& fb.ElemMatch(x => x.UserAccounts, ua => ua.UserId == userId);

			var findCursor = await clearances.FindAsync(filter);
			var clearance = await findCursor.FirstOrDefaultAsync();

			return clearance;
		}

		public async Task<ClearanceDto> GetClearanceByClearanceId(Guid clearanceId)
		{
			var result = await clearances.FindAsync(x => x.Id == clearanceId.ToString());
			return await result.SingleAsync();
		}
	}
}
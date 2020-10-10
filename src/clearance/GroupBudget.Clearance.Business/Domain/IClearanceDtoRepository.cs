using GroupBudget.Clearance.Messages.Dtos;
using System;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.Domain
{
	internal interface IClearanceDtoRepository
	{
		Task<Guid?> GetClearanceIdByAccountId(Guid value);
		Task<ClearanceDto> GetClearanceIdForUserOnPeriod(Guid value, DateTime startDate, DateTime endDate);
		Task Create(ClearanceDto accountDto);
		Task Update(ClearanceDto accountDto);
		Task<ClearanceDto> GetClearanceByClearanceId(Guid id);
	}
}
using GroupBudget.Account.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases.EventHandlers
{
	public interface IAccountDtoRepository
	{
		Task Create(AccountDto accountDto);
		Task<List<AccountDto>> GetAccountDtosByUserId(Guid ownerId);
	}
}
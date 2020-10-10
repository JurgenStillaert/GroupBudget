using GroupBudget.Clearance.Business.Domain;
using GroupBudget.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.Services
{
	public class UserSettingsService : IUserSettingsService
	{
		public Task<List<Guid>> GetUserGroupByUserId(UserId userId)
		{
			//Ask other bounded context for this information

			return Task.FromResult(new List<Guid> { Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Guid.Parse("d966e458-b102-4120-b023-21a375a810eb") });
		}
	}
}
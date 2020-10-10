using GroupBudget.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.Domain
{
	internal interface IUserSettingsService
	{
		Task<List<Guid>> GetUserGroupByUserId(UserId userId);
	}
}
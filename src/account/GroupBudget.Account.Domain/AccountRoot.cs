using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class AccountRoot : AggregateRoot
	{
		//public AccountRoot Create(AccountId accountId, UserId userId) { }

		protected override void EnsureValidation()
		{
			throw new NotImplementedException();
		}
	}
}

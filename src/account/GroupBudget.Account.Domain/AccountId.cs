using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class AccountId : AggregateId<AccountRoot>
	{
		public AccountId(Guid value) : base(value)
		{
		}

		public static AccountId FromGuid(Guid value) => new AccountId(value);

		public static AccountId FromString(string id) => new AccountId(Guid.Parse(id));
	}
}
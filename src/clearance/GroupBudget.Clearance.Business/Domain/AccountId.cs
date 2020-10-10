using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class AccountId : Value<AccountId>
	{
		public Guid Value { get; }

		protected AccountId(Guid value)
		{
			if (value == default)
			{
				throw new ArgumentNullException(nameof(value), "The account id cannot be empty");
			}

			Value = value;
		}

		public static AccountId FromGuid(Guid value) => new AccountId(value);
		public static AccountId FromString(string ownerId) => new AccountId(Guid.Parse(ownerId));


		public static implicit operator Guid(AccountId self) => self.Value;
	}
}
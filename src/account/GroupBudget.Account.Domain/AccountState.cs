using Codefondo.DDD.Kernel;

namespace GroupBudget.Account.Domain
{
	public class AccountState : Value<AccountState>
	{
		public AccountStateEnum Value { get; }

		public AccountState(AccountStateEnum value)
		{
			Value = value;
		}

		public static AccountState CreateOpen() => new AccountState(AccountStateEnum.Open);

		public static AccountState CreateClosed() => new AccountState(AccountStateEnum.Closed);

		public enum AccountStateEnum
		{
			Open,
			Closed
		}
	}
}
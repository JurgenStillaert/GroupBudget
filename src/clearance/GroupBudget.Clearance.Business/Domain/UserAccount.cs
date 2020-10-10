using System;

namespace GroupBudget.Clearance.Business.Domain
{
	internal class UserAccount
	{

		public UserAccount(
			Guid userId, 
			Guid? accountId, 
			UserAccountEnum userAccountEnum,
			decimal amount,
			string currencyCode)
		{
			if (userId == default)
			{
				throw new ArgumentNullException(nameof(userId), "User Id must have a value");
			}

			if (userAccountEnum == UserAccountEnum.Closed)
			{
				if (string.IsNullOrEmpty(currencyCode))
				{
					throw new ArgumentNullException(nameof(userId), "Currency code must have a value");
				}
			}

			UserId = userId;
			AccountId = accountId;
			State = userAccountEnum;
			Amount = amount;
			CurrencyCode = currencyCode;
		}

		public enum UserAccountEnum
		{
			Open,
			Closed
		}
		public Guid? AccountId { get; }
		public UserAccountEnum State { get; }
		public Guid UserId { get; }
		public decimal Amount { get; }
		public string CurrencyCode { get; }

		public UserAccount FromUserAccountClosed(decimal amount, string currencyCode) => new UserAccount(UserId, AccountId, UserAccountEnum.Closed, amount, currencyCode);

		public static UserAccount FromUserGuidAndAccountGuid(Guid userGuid, Guid? userAccount) => new UserAccount(userGuid, userAccount, UserAccountEnum.Open, 0, "");
	}
}
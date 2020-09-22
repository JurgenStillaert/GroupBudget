using Codefondo.DDD.Kernel;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Account.Domain
{
	public class AccountRoot : AggregateRoot
	{
		public UserId OwnerId { get; private set; }
		public Period Period { get; private set; }

		public static AccountRoot Create(AccountId accountId, UserId userId, Period period)
		{
			var account = new AccountRoot();

			account.Apply(new V1.AccountCreated(accountId, userId, period.StartDate, period.EndDate));

			return account;
		}

		protected override void EnsureValidation()
		{
			var valid =
				Id != null
				&& OwnerId != null
				&& Period != null
				&& Period.StartDate != default
				&& Period.EndDate != default
				&& Period.EndDate >= Period.StartDate;

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "Post-checks failed");
			}
		}

		private void Handle(V1.AccountCreated @event)
		{
			Id = @event.Id;
			OwnerId = UserId.FromGuid(@event.OwnerId);
			Period = Period.FromStartAndEndDate(@event.StartDate, @event.EndDate);
		}
	}
}
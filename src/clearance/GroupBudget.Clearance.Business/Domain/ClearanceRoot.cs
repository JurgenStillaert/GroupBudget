using Codefondo.DDD.Kernel;
using GroupBudget.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GroupBudget.Clearance.Messages.Events;


namespace GroupBudget.Clearance.Business.Domain
{
	internal class ClearanceRoot : AggregateRoot
	{
		public List<UserAccount> UserAccounts { get; private set; }
		public Period Period { get; private set; }
		public ClearanceState State { get; private set; }
		public PaymentSettlement PaymentSettlement { get; private set; } //only for 2 persons, normally this should be a list

		public async static Task<ClearanceRoot> Open(
			ClearanceId clearanceId,
			UserId userId,
			AccountId accountId,
			Period period,
			IUserSettingsService userSettingsService,
			IClearanceDtoRepository clearanceDtoRepository)
		{
			//Does this accountId already have a clearance?
			var clearanceIdAccountAlreadyExist = await clearanceDtoRepository.GetClearanceIdByAccountId(accountId.Value);

			if (clearanceIdAccountAlreadyExist != null)
			{
				throw new InvalidOperationException("Only one clearance can be created per account");
			}

			//Check if for the user is already a clearance on this period
			var clearanceDto = await clearanceDtoRepository.GetClearanceIdForUserOnPeriod(userId.Value, period.StartDate, period.EndDate);

			if (clearanceDto != null)
			{
				throw new InvalidOperationException("A clearance for this group already created. Try adding user accountId to existing clearance.");
			}

			//Get all the users of the group where the user is belonging to, remove this user
			var users = (await userSettingsService.GetUserGroupByUserId(userId)).Except(new List<Guid> { userId });

			//Build the dictionary for each user an account
			var userAccounts = new Dictionary<Guid, Guid?>();
			userAccounts.Add(userId.Value, accountId);

			foreach (var user in users)
			{
				userAccounts.Add(user, null);
			}

			var clearance = new ClearanceRoot();
			clearance.Apply(new V1.ClearanceCreated(clearanceId.Value, userAccounts, period.StartDate, period.EndDate));

			return clearance;
		}

		public void AddAccount(UserId userId, AccountId accountId)
		{
			if (!UserAccounts.Any(x => x.UserId == userId))
			{
				throw new InvalidOperationException("User must have an account in for this clearance");
			}

			Apply(new V1.AccountAdded(Id, userId.Value, accountId.Value));
		}

		public void CloseAccount(AccountId accountId, Payment payment)
		{
			var userAccount = UserAccounts.Single(x => x.AccountId == accountId);

			if (userAccount.State == UserAccount.UserAccountEnum.Closed)
			{
				throw new InvalidOperationException("Account is already closed");
			}

			Apply(new V1.AccountClosed(Id, accountId.Value, payment.Amount, payment.CurrencyCode));

			//If all acounts are closed now, we also should finalize the clearance
			if (!UserAccounts.Exists(x => x.State == UserAccount.UserAccountEnum.Open))
			{
				FinalizeClearance();
			}
		}

		private void FinalizeClearance()
		{
			//As test we only do 2 persons
			var payer = (Guid)default;
			var receiver = (Guid)default;
			decimal amountToPay = 0;

			var totalAmount = UserAccounts.Sum(x => x.Amount);

			var eachPersonShouldHavePaid = totalAmount / UserAccounts.Count;

			amountToPay = Math.Abs(eachPersonShouldHavePaid - UserAccounts[0].Amount);

			if ((eachPersonShouldHavePaid - UserAccounts[0].Amount) <= 0)
			{
				payer = UserAccounts[0].UserId;
				receiver = UserAccounts[1].UserId;
			}
			else
			{
				payer = UserAccounts[1].UserId;
				receiver = UserAccounts[0].UserId;
			}

			Apply(new V1.ClearanceFinalized(Id, payer, receiver, amountToPay, UserAccounts[0].CurrencyCode));
		}

		protected override void EnsureValidation()
		{
			var valid = Id != null
						&& Period != null
						&& UserAccounts != null;

			switch (State.Value)
			{
				case ClearanceState.ClearanceStateEnum.Open:
					//At least one account id must be present
					if (!UserAccounts.Exists(x => x.AccountId != null && x.AccountId != default))
					{
						valid = false;
					}

					//At least one user account must be open
					//todo

					//Clearance must be null
					if (PaymentSettlement != null)
					{
						valid = false;
					}

					break;

				case ClearanceState.ClearanceStateEnum.Closed:
					//Each user id and account id must be filled in
					if (!UserAccounts.Exists(x => x.AccountId == null || x.AccountId == default))
					{
						valid = false;
					}

					//All user accounts must be closed

					//Clearance may not be null
					if (PaymentSettlement == null)
					{
						valid = false;
					}

					break;

				default:
					throw new InvalidOperationException("State should always have a value");
			}

			if (!valid)
			{
				throw new DomainExceptions.InvalidEntityState(this, "Post-checks failed");
			}
		}

		private void Handle(V1.ClearanceCreated @event)
		{
			Id = ClearanceId.FromGuid(@event.ClearanceId);
			UserAccounts = @event.UserAccounts.Select(x => UserAccount.FromUserGuidAndAccountGuid(x.Key, x.Value)).ToList();
			Period = Period.FromStartAndEndDate(@event.StartDate, @event.EndDate);
			State = ClearanceState.CreateOpen();
		}

		private void Handle(V1.AccountAdded @event)
		{
			var userAccount = UserAccounts.Single(x => x.UserId == @event.userId);

			userAccount = UserAccount.FromUserGuidAndAccountGuid(@event.userId, @event.accountId);
		}

		private void Handle(V1.AccountClosed @event)
		{
			var userAccount = UserAccounts.Single(x => x.AccountId == @event.AccountId);
			userAccount = userAccount.FromUserAccountClosed(@event.Amount, @event.CurrencyCode);
		}

		private void Handle(V1.ClearanceFinalized @event)
		{
			PaymentSettlement = new PaymentSettlement(@event);
			State = ClearanceState.CreateClosed();
		}
	}
}
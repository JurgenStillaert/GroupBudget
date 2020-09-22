using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupBudget.Account.Domain.Tests
{
	public class AccountRootTest
	{
		[Test]
		public void Create_ValidParams_AccountReturned()
		{
			//Arrange
			var idGuid = Guid.Parse("051f8160-ce43-4ac0-b8c2-09707c2bcda3");
			var id = AccountId.FromGuid(idGuid);
			var ownerGuid = Guid.Parse("4bc0c0e9-7181-45e4-934d-e91c0e7bbd75");
			var ownerId = UserId.FromGuid(ownerGuid);
			var period = Period.FromMonth(2020, 9);

			//Act
			var account = AccountRoot.Create(id, ownerId, period);

			//Assert
			Assert.IsNotNull(account);
			Assert.AreEqual(idGuid, account.Id);
			Assert.AreEqual(ownerGuid, account.OwnerId.Value);
			Assert.AreEqual(DateTime.Parse("2020-09-01"), account.Period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-09-30"), account.Period.EndDate);
		}
	}
}

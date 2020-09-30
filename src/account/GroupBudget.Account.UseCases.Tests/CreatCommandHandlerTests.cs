using GroupBudget.Account.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static GroupBudget.Account.UseCases.CreateCommand;

namespace GroupBudget.Account.UseCases.Tests
{
	public class CreatCommandTests
	{
		[Test]
		public async Task Apply_ValidParams_AccountRootCreated()
		{
			//Arrange
			var id = Guid.Parse("4fc67e9a-1eec-4d08-9a38-3114a2ad07ba");
			var userId = Guid.Parse("c3216c1c-bef6-4a6a-ada7-06c038aa0da3");
			var month = 9;
			var year = 2020;
			var currency = "EUR";
			var command = new CreateCommand(id, userId, month, year, currency);

			var mockRepo = new Mock<IAccountRepository>();
			mockRepo.Setup(x => x.Save(It.IsAny<AccountRoot>())).Verifiable();

			var createCommandHandler = new CreateCommandHandler(mockRepo.Object);

			//Act
			await createCommandHandler.Apply(command);

			//Assert
			mockRepo.Verify();
		}
	}
}
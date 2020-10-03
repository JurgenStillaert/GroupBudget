using GroupBudget.Account.Domain;
using GroupBudget.Account.Dtos;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
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

			var accountDto = new CreateAccountDto
			{
				Currency = currency,
				Id = id,
				Month = month,
				Year = year,
				OwnerId = userId
			};

			var command = new CreateCommand(accountDto);

			var mockRepo = new Mock<IAccountRepository>();
			mockRepo.Setup(x => x.Save(It.IsAny<AccountRoot>())).Verifiable();

			var mockMediater = new Mock<IMediator>();
			mockMediater.Setup(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));

			var createCommandHandler = new CreateCommandHandler(mockRepo.Object, mockMediater.Object);

			//Act
			await createCommandHandler.Handle(command, CancellationToken.None);

			//Assert
			mockRepo.Verify();
		}
	}
}
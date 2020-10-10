using GroupBudget.Clearance.Messages.Commands;
using Hangfire;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Clearance.Business.UseCases.EventHandlers
{
	public class AccountCreatedNotification : INotificationHandler<V1.AccountCreated>
	{
		private readonly IBackgroundJobClient backgroundJobClient;
		private readonly IMediator mediator;

		public AccountCreatedNotification(IMediator mediator, IBackgroundJobClient backgroundJobClient)
		{
			this.mediator = mediator;
			this.backgroundJobClient = backgroundJobClient;
		}

		public Task Handle(V1.AccountCreated notification, CancellationToken cancellationToken)
		{
			backgroundJobClient.Enqueue(() => Publish(notification));

			return Task.CompletedTask;
		}

		public Task Publish(V1.AccountCreated notification)
		{
			return mediator.Send(new OpenClearanceCommand(Guid.NewGuid(), notification.OwnerId, notification.AccountId, notification.StartDate, notification.EndDate));
		}
	}
}
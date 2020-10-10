using GroupBudget.Clearance.Messages.Commands;
using Hangfire;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Clearance.Business.UseCases.EventHandlers
{
	public class AccountClosedNotification : INotificationHandler<V1.AccountClosed>
	{
		private readonly IBackgroundJobClient backgroundJobClient;
		private readonly IMediator mediator;

		public AccountClosedNotification(IMediator mediator, IBackgroundJobClient backgroundJobClient)
		{
			this.mediator = mediator;
			this.backgroundJobClient = backgroundJobClient;
		}

		public Task Handle(V1.AccountClosed notification, CancellationToken cancellationToken)
		{
			backgroundJobClient.Enqueue(() => Publish(notification));

			return Task.CompletedTask;
		}

		public Task Publish(V1.AccountClosed notification)
		{
			return mediator.Send(new CloseAccountCommand(notification.AccountId));
		}
	}
}
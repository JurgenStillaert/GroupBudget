using Hangfire;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events.V1;

namespace GroupBudget.Account.UseCases.EventHandlers
{
	public class BackEndJobExampleNotification : INotificationHandler<AccountCreated>
	{
		private readonly IBackgroundJobClient backgroundJobClient;

		public BackEndJobExampleNotification(IBackgroundJobClient backgroundJobClient)
		{
			this.backgroundJobClient = backgroundJobClient;
		}

		public Task Handle(AccountCreated @event, CancellationToken cancellationToken)
		{
			backgroundJobClient.Enqueue(() => Console.WriteLine("This is a background job executing"));

			return Task.CompletedTask;
		}
	}
}
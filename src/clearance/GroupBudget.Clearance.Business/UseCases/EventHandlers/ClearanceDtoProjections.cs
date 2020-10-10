using GroupBudget.Clearance.Business.Domain;
using GroupBudget.Clearance.Messages.Dtos;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Clearance.Messages.Events;

namespace GroupBudget.Clearance.Business.UseCases.EventHandlers
{
	internal static class ClearanceDtoProjections
	{
		internal class ClearanceCreatedNotification : INotificationHandler<V1.ClearanceCreated>
		{
			private readonly IClearanceDtoRepository clearanceDtoRepository;

			public ClearanceCreatedNotification(IClearanceDtoRepository clearanceDtoRepository)
			{
				this.clearanceDtoRepository = clearanceDtoRepository;
			}

			public async Task Handle(V1.ClearanceCreated @event, CancellationToken cancellationToken)
			{
				var clearanceDto = new ClearanceDto
				{
					Id = @event.ClearanceId.ToString(),
					StartDate = @event.StartDate,
					EndDate = @event.EndDate,
					UserAccounts = @event.UserAccounts.Select(x => new ClearanceDto.UserAccountDto
					{
						AccountId = x.Key,
						UserId = x.Value,
						Amount = null,
						CurrencyCode = null
					}).ToList()
				};

				await clearanceDtoRepository.Create(clearanceDto);
			}
		}

		internal class AccountClosedNotification : INotificationHandler<V1.AccountClosed>
		{
			private readonly IClearanceDtoRepository clearanceDtoRepository;

			public AccountClosedNotification(IClearanceDtoRepository clearanceDtoRepository)
			{
				this.clearanceDtoRepository = clearanceDtoRepository;
			}

			public async Task Handle(V1.AccountClosed @event, CancellationToken cancellationToken)
			{
				var clearanceDto = await clearanceDtoRepository.GetClearanceByClearanceId(@event.ClearanceId);

				if (clearanceDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				var userAccount = clearanceDto.UserAccounts.Single(x => x.AccountId == @event.AccountId);

				userAccount.Amount = @event.Amount;
				userAccount.CurrencyCode = @event.CurrencyCode;

				await clearanceDtoRepository.Update(clearanceDto);
			}
		}

		internal class ClearanceFinalizedNotification : INotificationHandler<V1.ClearanceFinalized>
		{
			private readonly IClearanceDtoRepository clearanceDtoRepository;

			public ClearanceFinalizedNotification(IClearanceDtoRepository clearanceDtoRepository)
			{
				this.clearanceDtoRepository = clearanceDtoRepository;
			}

			public async Task Handle(V1.ClearanceFinalized @event, CancellationToken cancellationToken)
			{
				var clearanceDto = await clearanceDtoRepository.GetClearanceByClearanceId(@event.ClearanceId);

				if (clearanceDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				clearanceDto.Settlement = new ClearanceDto.PaymentSettlement
				{
					Payer = @event.Payer,
					Receiver = @event.Receiver,
					Amount = @event.Amount,
					CurrencyCode = @event.CurrencyCode
				};

				await clearanceDtoRepository.Update(clearanceDto);
			}
		}
	}
}
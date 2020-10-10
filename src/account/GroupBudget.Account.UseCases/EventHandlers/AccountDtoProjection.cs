using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events;

namespace GroupBudget.Account.UseCases.EventHandlers
{
	public static class AccountDtoProjections
	{
		public class AccountCreatedNotification : INotificationHandler<V1.AccountCreated>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public AccountCreatedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(V1.AccountCreated @event, CancellationToken cancellationToken)
			{
				var periodeName = $"{@event.StartDate.ToString("MMMM")} {@event.StartDate.Year}";

				var accountDto = new AccountDto(
										@event.AccountId,
										@event.OwnerId,
										periodeName,
										@event.CurrencyCode,
										new List<AccountItemDto>(),
										false,
										@event.StartDate,
										@event.EndDate);

				await accountDtoRepository.Create(accountDto);
			}
		}

		public class BookingAddedToAccountNotification : INotificationHandler<V1.BookingAddedToAccount>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingAddedToAccountNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(V1.BookingAddedToAccount @event, CancellationToken cancellationToken)
			{
				var accountDto = await accountDtoRepository.GetAccountByAccountId(@event.AccountId);

				if (accountDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				if (accountDto.Items == null)
				{
					accountDto.Items = new List<AccountItemDto>();
				}

				accountDto.Items.Add(new AccountItemDto(@event.BookingId, @event.Date, @event.Amount, @event.Description));

				accountDto.TotalSpent = accountDto.Items.Sum(x => x.Amount);

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class BookingChangedNotification : INotificationHandler<V1.BookingChanged>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingChangedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(V1.BookingChanged @event, CancellationToken cancellationToken)
			{
				var accountDto = await accountDtoRepository.GetAccountByAccountId(@event.AccountId);

				if (accountDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				if (accountDto.Items == null)
				{
					accountDto.Items = new List<AccountItemDto>();
				}

				var bookingItem = accountDto.Items.Single(x => x.BookingId == @event.BookingId);

				bookingItem.Amount = @event.Amount;
				bookingItem.Date = @event.Date;
				bookingItem.Description = @event.Description;

				accountDto.TotalSpent = accountDto.Items.Sum(x => x.Amount);

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class BookingRemovedFromAccountNotification : INotificationHandler<V1.BookingRemovedFromAccount>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingRemovedFromAccountNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(V1.BookingRemovedFromAccount @event, CancellationToken cancellationToken)
			{
				var accountDto = await accountDtoRepository.GetAccountByAccountId(@event.AccountId);

				if (accountDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				if (accountDto.Items == null)
				{
					accountDto.Items = new List<AccountItemDto>();
				}

				var bookingItem = accountDto.Items.Single(x => x.BookingId == @event.BookingId);

				accountDto.Items.Remove(bookingItem);

				accountDto.TotalSpent = accountDto.Items.Sum(x => x.Amount);

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class AccountClosedNotification : INotificationHandler<V1.AccountClosed>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public AccountClosedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(V1.AccountClosed @event, CancellationToken cancellationToken)
			{
				var accountDto = await accountDtoRepository.GetAccountByAccountId(@event.AccountId);

				if (accountDto == null)
				{
					throw new NullReferenceException("No accountDto has been found");
				}

				accountDto.MonthIsClosed = true;

				await accountDtoRepository.Update(accountDto);
			}
		}
	}
}
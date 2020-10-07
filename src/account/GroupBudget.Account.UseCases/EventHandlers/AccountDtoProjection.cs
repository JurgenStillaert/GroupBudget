using GroupBudget.Account.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events.V1;

namespace GroupBudget.Account.UseCases.EventHandlers
{
	public static class AccountDtoProjections
	{
		public class AccountCreatedNotification : INotificationHandler<AccountCreated>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public AccountCreatedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(AccountCreated @event, CancellationToken cancellationToken)
			{
				var periodeName = $"{@event.StartDate.ToString("MMMM")} {@event.StartDate.Year}";

				var accountDto = new AccountDto(
										@event.Id,
										@event.OwnerId,
										periodeName,
										$"0 {@event.CurrencyCode}",
										new List<AccountItemDto>(),
										false,
										@event.StartDate,
										@event.EndDate);

				await accountDtoRepository.Create(accountDto);
			}
		}

		public class BookingAddedToAccountNotification : INotificationHandler<BookingAddedToAccount>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingAddedToAccountNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(BookingAddedToAccount @event, CancellationToken cancellationToken)
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

				accountDto.Items.Add(new AccountItemDto(@event.BookingId, @event.Date, $"{@event.Amount} {@event.CurrencyCode}", @event.Description));

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class BookingChangedNotification : INotificationHandler<BookingChanged>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingChangedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(BookingChanged @event, CancellationToken cancellationToken)
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

				bookingItem.Amount = $"{@event.Amount} {@event.CurrencyCode}";
				bookingItem.Date = @event.Date;
				bookingItem.Description = @event.Description;

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class BookingRemovedFromAccountNotification : INotificationHandler<BookingRemovedFromAccount>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public BookingRemovedFromAccountNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(BookingRemovedFromAccount @event, CancellationToken cancellationToken)
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

				await accountDtoRepository.Update(accountDto);
			}
		}

		public class AccountClosedNotification : INotificationHandler<AccountClosed>
		{
			private readonly IAccountDtoRepository accountDtoRepository;

			public AccountClosedNotification(IAccountDtoRepository accountDtoRepository)
			{
				this.accountDtoRepository = accountDtoRepository;
			}

			public async Task Handle(AccountClosed @event, CancellationToken cancellationToken)
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
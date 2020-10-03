using GroupBudget.Account.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static GroupBudget.Account.Messages.Events.V1;

namespace GroupBudget.Account.UseCases.EventHandlers
{
	public class AccountDtoProjectionNotification : INotificationHandler<AccountCreated>
	{
		private readonly IAccountDtoRepository accountDtoRepository;

		public AccountDtoProjectionNotification(IAccountDtoRepository accountDtoRepository)
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
									new System.Collections.Generic.List<AccountItemDto>(),
									false,
									@event.StartDate,
									@event.EndDate);

			await accountDtoRepository.Create(accountDto);
		}
	}
}
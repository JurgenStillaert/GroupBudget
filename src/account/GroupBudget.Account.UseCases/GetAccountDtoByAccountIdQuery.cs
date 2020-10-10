using GroupBudget.Account.Messages.Dtos;
using GroupBudget.Account.Messages.Queries;
using GroupBudget.Account.UseCases.EventHandlers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class GetAccountDtoByAccountIdQueryHandler : IRequestHandler<GetAccountDtoByAccountIdQuery, AccountDto>
	{
		private readonly IAccountDtoRepository accountDtoRepository;

		public GetAccountDtoByAccountIdQueryHandler(IAccountDtoRepository accountDtoRepository)
		{
			this.accountDtoRepository = accountDtoRepository;
		}

		public async Task<AccountDto> Handle(GetAccountDtoByAccountIdQuery request, CancellationToken cancellationToken)
		{
			var accountDtos = await accountDtoRepository.GetAccountByAccountId(request.AccountId);
			return accountDtos;
		}
	}
}
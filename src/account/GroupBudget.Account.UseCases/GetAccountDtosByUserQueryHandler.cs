using GroupBudget.Account.Messages.Dtos;
using GroupBudget.Account.Messages.Queries;
using GroupBudget.Account.UseCases.EventHandlers;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class GetAccountDtosByUserQueryHandler : IRequestHandler<GetAccountDtosByUserQuery, IEnumerable<AccountDto>>
	{
		private readonly IAccountDtoRepository accountDtoRepository;

		public GetAccountDtosByUserQueryHandler(IAccountDtoRepository accountDtoRepository)
		{
			this.accountDtoRepository = accountDtoRepository;
		}

		public async Task<IEnumerable<AccountDto>> Handle(GetAccountDtosByUserQuery request, CancellationToken cancellationToken)
		{
			var accountDtos = await accountDtoRepository.GetAccountDtosByUserId(request.OwnerId);
			return accountDtos.OrderBy(x => x.StartDate);
		}
	}
}
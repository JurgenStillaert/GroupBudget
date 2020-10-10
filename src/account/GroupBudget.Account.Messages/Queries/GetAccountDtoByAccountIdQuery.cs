using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;

namespace GroupBudget.Account.Messages.Queries
{
	public sealed class GetAccountDtoByAccountIdQuery : IRequest<AccountDto>
	{
		public GetAccountDtoByAccountIdQuery(Guid accountId)
		{
			AccountId = accountId;
		}

		public Guid AccountId { get; }
	}
}
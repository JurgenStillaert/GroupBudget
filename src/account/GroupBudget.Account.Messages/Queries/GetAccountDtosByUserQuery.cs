using GroupBudget.Account.Messages.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace GroupBudget.Account.Messages.Queries
{
	public sealed class GetAccountDtosByUserQuery : IRequest<IEnumerable<AccountDto>>
	{
		public GetAccountDtosByUserQuery(Guid ownerId)
		{
			if (ownerId == null || ownerId == default)
			{
				throw new ArgumentNullException(nameof(ownerId), "Cannot search for empty owner id");
			}

			OwnerId = ownerId;
		}

		public Guid OwnerId { get; }
	}
}
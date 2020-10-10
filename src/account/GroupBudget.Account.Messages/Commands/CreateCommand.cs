using GroupBudget.Account.Messages.Dtos;
using MediatR;

namespace GroupBudget.Account.Messages.Commands
{
	public sealed class CreateCommand : IRequest
	{
		public CreateCommand(CreateAccountDto accountDto)
		{
			AccountDto = accountDto;
		}

		public CreateAccountDto AccountDto { get; }
	}
}
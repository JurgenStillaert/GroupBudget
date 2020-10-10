using Codefondo.UseCase.Kernel;
using GroupBudget.Clearance.Business.Domain;
using GroupBudget.Clearance.Messages.Commands;
using GroupBudget.SharedKernel;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.UseCases
{
	internal class CloseAccountCommandHandler : UpdateCommandHandler<CloseAccountCommand, ClearanceRoot>
	{
		private readonly IClearanceDtoRepository clearanceDtoRepository;
		private readonly IAccountDtoService accountDtoRepository;

		public CloseAccountCommandHandler(
			IRepository<ClearanceRoot> clearanceRepository,
			IClearanceDtoRepository clearanceDtoRepository,
			IMediator mediator,
			IAccountDtoService accountDtoService)
			: base(clearanceRepository, mediator)
		{
			this.accountDtoRepository = accountDtoService;
			this.clearanceDtoRepository = clearanceDtoRepository;
		}

		protected async override void PreHandle(CloseAccountCommand command)
		{
			AggregateId = ClearanceId.FromGuid((await clearanceDtoRepository.GetClearanceIdByAccountId(command.AccountId)).Value);
		}

		protected async override Task<ClearanceRoot> Apply(CloseAccountCommand command, CancellationToken cancellationToken)
		{
			var account = AggregateRoot.UserAccounts.Single(x => x.AccountId == command.AccountId);

			if (account.State == UserAccount.UserAccountEnum.Closed)
			{
				throw new InvalidOperationException("You cannot close an account that is already closed");
			}

			//Get the total amount spent on this account
			var totalSpent = await accountDtoRepository.TotalAmountSpentOnAccount(command.AccountId);

			AggregateRoot.CloseAccount(AccountId.FromGuid(command.AccountId), Payment.FromDecimal(totalSpent.Amount, totalSpent.CurrencyCode));

			return AggregateRoot;
		}
	}
}
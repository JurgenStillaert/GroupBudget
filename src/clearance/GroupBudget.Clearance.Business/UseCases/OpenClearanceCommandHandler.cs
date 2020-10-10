using Codefondo.UseCase.Kernel;
using GroupBudget.Clearance.Business.Domain;
using GroupBudget.Clearance.Messages.Commands;
using GroupBudget.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kernel = GroupBudget.SharedKernel;

namespace GroupBudget.Clearance.Business.UseCases
{
	internal class OpenClearanceCommandHandler : CreateCommandHandler<OpenClearanceCommand, ClearanceRoot>
	{
		private readonly IClearanceDtoRepository clearanceDtoRepository;
		private readonly IUserSettingsService userSettingsService;

		public OpenClearanceCommandHandler(
			IRepository<ClearanceRoot> clearanceRepository,
			IMediator mediator,
			IUserSettingsService userSettingsService,
			IClearanceDtoRepository clearanceDtoRepository)
			: base(clearanceRepository, mediator)
		{
			this.userSettingsService = userSettingsService;
			this.clearanceDtoRepository = clearanceDtoRepository;
		}

		protected async override Task<ClearanceRoot> Apply(OpenClearanceCommand command, CancellationToken cancellationToken)
		{
			var clearanceDto = await clearanceDtoRepository.GetClearanceIdForUserOnPeriod(command.UserId, command.StartDate, command.EndDate);

			if (clearanceDto == null)
			{
				var clearance = await ClearanceRoot.Open(
						Domain.ClearanceId.FromGuid(command.ClearanceId),
						Kernel.UserId.FromGuid(command.UserId),
						Domain.AccountId.FromGuid(command.AccountId),
						Kernel.Period.FromStartAndEndDate(command.StartDate, command.EndDate),
						userSettingsService,
						clearanceDtoRepository
					);

				return clearance;
			}
			else
			{
				AggregateRoot = await GetAggregateFromRepo();

				AggregateRoot.AddAccount(UserId.FromGuid(command.UserId), AccountId.FromGuid(command.AccountId));

				return AggregateRoot;
			}
		}
	}
}
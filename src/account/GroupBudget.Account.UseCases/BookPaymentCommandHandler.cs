using Codefondo.UseCase.Kernel;
using GroupBudget.Account.Domain;
using GroupBudget.Account.Messages.Commands;
using GroupBudget.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GroupBudget.Account.UseCases
{
	internal class BookPaymentCommandHandler : UpdateCommandHandler<BookPaymentCommand, AccountRoot>
	{
		public BookPaymentCommandHandler(IRepository<AccountRoot> repo, IMediator mediator) : base(repo, mediator)
		{
		}

		protected override void PreHandle(BookPaymentCommand command)
		{
			AggregateId = AccountId.FromGuid(command.AccountRootId);
		}

		protected async override Task<AccountRoot> Apply(BookPaymentCommand command, CancellationToken cancellationToken)
		{
			AggregateRoot.BookPayment(
					BookingId.FromGuid(command.AccountItemDto.BookingId),
					Payment.FromDecimal(command.AccountItemDto.Amount, AggregateRoot.Currency.Value),
					BookingDate.FromDate(command.AccountItemDto.Date),
					Description.FromString(command.AccountItemDto.Description));

			return await Task.FromResult(AggregateRoot);
		}
	}
}
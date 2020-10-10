using GroupBudget.Account.Messages.Commands;
using GroupBudget.Account.Messages.Dtos;
using GroupBudget.Account.Messages.Queries;
using GroupBudget.Account.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.Account.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IMediator mediator;

		public AccountController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts([FromQuery] Guid userGuid)
		{
			return Ok(await mediator.Send(new GetAccountDtosByUserQuery(userGuid)));
		}

		[HttpPost]
		public async Task<ActionResult<AccountDto>> Create([FromBody] CreateAccountDto accountDto)
		{
			accountDto.Id = Guid.NewGuid();

			await mediator.Send(new CreateCommand(accountDto));

			return Ok(accountDto);
		}

		[HttpPost]
		[Route("{accountId}/booking/")]
		public async Task<ActionResult<AccountItemDto>> PostNewBooking([FromRoute] Guid accountId, [FromBody] AccountItemDto booking)
		{
			booking.BookingId = Guid.NewGuid();

			await mediator.Send(new BookPaymentCommand(accountId, booking));

			return Ok(booking);
		}

		[HttpPut]
		[Route("{accountId}/booking/{bookingId}")]
		public async Task<ActionResult<AccountItemDto>> PutBooking(
			[FromRoute] Guid accountId,
			[FromRoute] Guid bookingId,
			[FromBody] AccountItemDto booking)
		{
			await mediator.Send(new ChangePaymentCommand(accountId, bookingId, booking));

			booking.BookingId = bookingId;
			return Ok(booking);
		}

		[HttpPut]
		[Route("{accountId}")]
		public async Task<IActionResult> CloseBooking([FromRoute] Guid accountId)
		{
			await mediator.Send(new CloseAccountCommand(accountId));

			return Ok();
		}

		[HttpDelete]
		[Route("/{accountId}/booking/{bookingId}")]
		public async Task<IActionResult> DeleteBooking([FromRoute] Guid accountId, [FromRoute] Guid bookingId)
		{
			await mediator.Send(new RemovePaymentCommand(accountId, bookingId));

			return Ok();
		}
	}
}
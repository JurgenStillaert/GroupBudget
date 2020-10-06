using GroupBudget.Account.Dtos;
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

		[HttpPost]
		public async Task<ActionResult<AccountDto>> Create([FromBody] CreateAccountDto accountDto)
		{
			accountDto.Id = Guid.NewGuid();

			await mediator.Send(new CreateCommand(accountDto));

			return Ok(accountDto);
		}

		[HttpGet]
		[Route("/user/{userGuid}")]
		public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsByUser([FromRoute] Guid userGuid)
		{
			return Ok(await mediator.Send(new GetAccountDtosByUserQuery(userGuid)));
		}

		[HttpPost]
		[Route("/{accountId}/booking/")]
		public async Task<ActionResult<AccountItemDto>> PostNewBooking([FromRoute]Guid accountId, [FromBody] AccountItemDto booking)
		{
			booking.BookingId = Guid.NewGuid();

			await mediator.Send(new BookPaymentCommand(accountId, booking));

			return Ok(booking);
		}
	}
}
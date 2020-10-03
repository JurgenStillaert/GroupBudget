using GroupBudget.Account.Dtos;
using GroupBudget.Account.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupBudget.WebApi.Controllers
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
		public async Task<ActionResult<AccountDto>> Create([FromBody]CreateAccountDto accountDto)
		{
			accountDto.Id = Guid.NewGuid();

			await mediator.Send(new CreateCommand(accountDto));

			return Ok(accountDto);
		}

		[HttpGet]
		[Route("/user/{userGuid}")]
		public async Task<IEnumerable<AccountDto>> GetAccountsByUser([FromQuery]Guid userGuid)
		{
			return await mediator.Send(new GetAccountDtosByUserQuery(userGuid));
		}
	}
}
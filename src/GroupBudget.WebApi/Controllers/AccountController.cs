using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupBudget.Account.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
		public Task Create()
		{
			return mediator.Send(new CreateCommand(Guid.NewGuid(), Guid.NewGuid(), 9, 2020, "EUR"));
		}
	}
}

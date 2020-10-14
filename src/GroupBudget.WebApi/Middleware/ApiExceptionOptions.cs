using Microsoft.AspNetCore.Http;
using System;

namespace GroupBudget.WebApi.Middleware
{
	public class ApiExceptionOptions
	{
		public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
	}
}
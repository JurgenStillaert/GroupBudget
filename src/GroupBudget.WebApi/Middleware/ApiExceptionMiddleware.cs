using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GroupBudget.WebApi.Middleware
{
	public class ApiExceptionMiddleware
	{
		private readonly ILogger<ApiExceptionMiddleware> logger;
		private readonly RequestDelegate next;
		private readonly ApiExceptionOptions options;

		public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
		{
			this.options = options;
			this.next = next;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex, options);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception ex, ApiExceptionOptions options)
		{
			var error = new ApiError
			{
				Id = Guid.NewGuid().ToString(),
				Status = (short)HttpStatusCode.InternalServerError,
				Title = "Some kind of error occurred in the API. Please use the id and contact our support team if the problem persists."
			};

			options.AddResponseDetails?.Invoke(context, ex, error);

			var innerExMessage = GetInnerMostExceptionMessage(ex);

			logger.LogError(ex, "BADNESS!!! " + innerExMessage + "-- {errorId}.", error.Id);

			var result = JsonConvert.SerializeObject(error);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return context.Response.WriteAsync(result);
		}

		private object GetInnerMostExceptionMessage(Exception ex)
		{
			if (ex.InnerException != null)
			{
				return GetInnerMostExceptionMessage(ex.InnerException);
			}
			return ex.Message;
		}
	}
}
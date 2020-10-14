using GroupBudget.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GroupBudget.WebApi.Filters
{
	public class TrackActionPerformanceFilter : IActionFilter
	{
		private readonly ILogger<TrackActionPerformanceFilter> logger;
		private Stopwatch timer;

		public TrackActionPerformanceFilter(ILogger<TrackActionPerformanceFilter> logger)
		{
			this.logger = logger;
		}
		public void OnActionExecuting(ActionExecutingContext context)
		{
			timer = new Stopwatch();
			timer.Start();
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			timer.Stop();

			if (context.Exception == null)
			{
				logger.LogRoutePerformance(context.HttpContext.Request.Path, context.HttpContext.Request.Method, timer.ElapsedMilliseconds);
			}
		}

	}
}

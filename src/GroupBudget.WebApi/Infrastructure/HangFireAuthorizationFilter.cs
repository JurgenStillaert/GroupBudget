using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace GroupBudget.WebApi.Infrastructure
{
	public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize([NotNull] DashboardContext context)
		{
			return true;
		}
	}
}
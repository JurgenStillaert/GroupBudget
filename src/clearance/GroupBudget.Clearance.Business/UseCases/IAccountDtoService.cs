using System;
using System.Threading.Tasks;

namespace GroupBudget.Clearance.Business.UseCases
{
	public interface IAccountDtoService
	{
		Task<TotalSpent> TotalAmountSpentOnAccount(Guid accountId);
	}

	public class TotalSpent
	{
		public TotalSpent(decimal amount, string currencyCode)
		{
			Amount = amount;
			CurrencyCode = currencyCode;
		}

		public decimal Amount { get; }
		public string CurrencyCode { get; }
	}
}
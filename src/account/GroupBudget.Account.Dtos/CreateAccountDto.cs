using System;

namespace GroupBudget.Account.Dtos
{
	public class CreateAccountDto
	{
		public Guid Id { get; set; }
		public Guid OwnerId { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Currency { get; set; }
	}
}
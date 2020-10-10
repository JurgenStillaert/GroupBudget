using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GroupBudget.Clearance.Messages.Dtos
{
	[BsonIgnoreExtraElements]
	public class ClearanceDto
	{
		[BsonId]
		public string Id { get; set; }
		[BsonElement("startdate")]
		public DateTime StartDate { get; set; }
		[BsonElement("enddate")]
		public DateTime EndDate { get; set; }
		[BsonElement("useraccounts")]
		public List<UserAccountDto> UserAccounts { get; set; }
		[BsonElement("settlement")]
		public PaymentSettlement Settlement { get; set; }

		public class UserAccountDto
		{
			[BsonElement("accountid")]
			public Guid AccountId { get; set; }
			[BsonElement("userid")]
			public Guid? UserId { get; set; }
			[BsonElement("amount")]
			public decimal? Amount { get; set; }
			[BsonElement("currencycode")]
			public string CurrencyCode { get; set; }
		}

		public class PaymentSettlement
		{
			[BsonElement("payer")]
			public Guid Payer { get; set; }
			[BsonElement("receiver")]
			public Guid Receiver { get; set; }
			[BsonElement("amount")]
			public Decimal Amount { get; set; }
			[BsonElement("currencycode")]
			public string CurrencyCode { get; set; }
		}
	}
}
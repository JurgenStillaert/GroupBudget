using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GroupBudget.Account.Messages.Dtos
{
	[BsonIgnoreExtraElements]
	public class AccountDto
	{
		public AccountDto(
			Guid id,
			Guid ownerId,
			string periodName,
			string currencyCode,
			List<AccountItemDto> items,
			bool monthIsClosed,
			DateTime startDate,
			DateTime endDate)
		{
			Id = id.ToString();
			OwnerId = ownerId.ToString();
			PeriodName = periodName;
			CurrencyCode = currencyCode;
			Items = items;
			MonthIsClosed = monthIsClosed;
			StartDate = startDate;
			EndDate = endDate;
		}

		[BsonId]
		public string Id { get; set; }

		[BsonElement("owner")]
		public string OwnerId { get; set; }

		[BsonElement("period")]
		public string PeriodName { get; set; }

		[BsonElement("currency")]
		public string CurrencyCode { get; set; }

		[BsonElement("spent")]
		public decimal TotalSpent { get; set; }

		[BsonElement("items")]
		public List<AccountItemDto> Items { get; set; }

		[BsonElement("closed")]
		public bool MonthIsClosed { get; set; }

		[BsonElement("startdate")]
		public DateTime StartDate { get; set; }

		[BsonElement("enddate")]
		public DateTime EndDate { get; set; }
	}

	public class AccountItemDto
	{
		public AccountItemDto(Guid bookingId, DateTime date, decimal amount, string description)
		{
			Date = date;
			Amount = amount;
			Description = description;
			BookingId = bookingId;
		}

		//Constructor to fullfill serializer
		public AccountItemDto()
		{

		}

		[BsonElement("bookingId")]
		public Guid BookingId { get; set; }
		[BsonElement("date")]
		public DateTime Date { get; set; }
		[BsonElement("amount")]
		public decimal Amount { get; set; }
		[BsonElement("description")]
		public string Description { get; set; }
	}
}
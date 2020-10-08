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
			string totalSpent,
			List<AccountItemDto> items,
			bool monthIsClosed,
			DateTime startDate,
			DateTime endDate)
		{
			Id = id.ToString();
			OwnerId = ownerId.ToString();
			PeriodName = periodName;
			TotalSpent = totalSpent;
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

		[BsonElement("spent")]
		public string TotalSpent { get; set; }

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
		public AccountItemDto(Guid bookingId, DateTime date, string amount, string description)
		{
			Date = date;
			Amount = amount;
			Description = description;
			BookingId = bookingId;
		}

		public AccountItemDto()
		{
		}

		public Guid BookingId { get; set; }
		public DateTime Date { get; set; }
		public string Amount { get; set; }
		public string Description { get; set; }
	}
}
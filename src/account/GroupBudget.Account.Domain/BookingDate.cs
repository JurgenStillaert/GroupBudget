using System;

namespace GroupBudget.Account.Domain
{
	public class BookingDate
	{
		public DateTime Value { get; }

		protected BookingDate(DateTime date)
		{
			if (date == default)
				throw new ArgumentNullException(nameof(date), "Booking date cannot be empty");

			Value = date;
		}

		public static BookingDate FromDate(DateTime date)
			=> new BookingDate(date);

		public static BookingDate FromString(string date)
			=> new BookingDate(DateTime.Parse(date));
	}
}
using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class BookingId : Value<BookingId>
	{
		public Guid Value { get; }

		protected BookingId(Guid value)
		{
			if (value == default)
				throw new ArgumentNullException("Booking id must have a value", nameof(value));

			Value = value;
		}

		public static BookingId FromGuid(Guid value) => new BookingId(value);
	}
}
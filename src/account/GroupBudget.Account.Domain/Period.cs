using Codefondo.DDD.Kernel;
using System;
using System.Globalization;

namespace GroupBudget.Account.Domain
{
	public class Period : Value<Period>
	{
		public DateTime StartDate { get; }
		public DateTime EndDate { get; }

		protected Period(DateTime startDate, DateTime endDate)
		{
			if (startDate == default)
			{
				throw new ArgumentNullException(nameof(startDate), "The startdate of the period cannot be empty");
			}
			if (endDate == default)
			{
				throw new ArgumentNullException(nameof(endDate), "The enddate of the period cannot be empty");
			}
			if (startDate > endDate)
			{
				throw new ArgumentException("End date cannot earlier than start date");
			}

			StartDate = startDate.Date;
			EndDate = endDate.Date;
		}

		public static Period FromStartAndEndDate(DateTime startDate, DateTime endDate) => new Period(startDate, endDate);

		public static Period FromStartDateAndDuration(DateTime startDate, DurationEnum durationEnum)
		{
			DateTime endDate;

			if (startDate == default)
			{
				throw new ArgumentNullException(nameof(startDate), "The startdate of the period cannot be empty");
			}

			switch (durationEnum)
			{
				case DurationEnum.Day:
					endDate = startDate.AddDays(1);
					break;

				case DurationEnum.Week:
					endDate = startDate.Add(TimeSpan.FromDays(7));
					break;

				case DurationEnum.Month:
					endDate = startDate.AddMonths(1);
					break;

				case DurationEnum.Trimester:
					endDate = startDate.AddMonths(3);
					break;

				case DurationEnum.Semester:
					endDate = startDate.AddMonths(6);
					break;

				case DurationEnum.Year:
					endDate = startDate.AddYears(1);
					break;

				default:
					throw new InvalidOperationException("Not implemented DurationEnum detected");
			}

			return new Period(startDate, endDate.Subtract(TimeSpan.FromDays(1)));
		}

		public static Period FromWeekNumber(int year, int weekNumber)
		{
			if (year < 1900 || year > 2100)
			{
				throw new ArgumentOutOfRangeException(nameof(year), "Year is not in a reasonable range (1900-2100)");
			}

			var startDate = ISOWeek.ToDateTime(year, weekNumber, DayOfWeek.Monday);
			var endDate = startDate.AddDays(6);

			return new Period(startDate, endDate);
		}

		public static Period FromMonth(int year, int monthNumber)
		{
			if (year < 1900 || year > 2100)
			{
				throw new ArgumentOutOfRangeException(nameof(year), "Year is not in a reasonable range (1900-2100)");
			}

			if (monthNumber < 1 || monthNumber > 12)
			{
				throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month is not in a reasonable range (1-12)");
			}

			var startDate = new DateTime(year, monthNumber, 1);
			var endDate = startDate.AddDays(DateTime.DaysInMonth(year, monthNumber) - 1);

			return new Period(startDate, endDate);
		}

		public enum DurationEnum
		{
			Day,
			Week,
			Month,
			Trimester,
			Semester,
			Year
		}
	}
}
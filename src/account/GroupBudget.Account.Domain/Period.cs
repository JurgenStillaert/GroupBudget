using Codefondo.DDD.Kernel;
using System;

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

			StartDate = startDate;
			EndDate = endDate;
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
			throw new NotImplementedException();
		}

		public static Period FromMonth(int year, int weekNumber)
		{
			throw new NotImplementedException();
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
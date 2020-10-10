using GroupBudget.SharedKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupBudget.Account.Domain.Tests
{
	public class PeriodTests
	{
		[Test]
		public void FromStartAndEndDate_ValidDates_PeriodWithDates()
		{
			//Arrange
			var startDate = DateTime.Parse("2020-01-01");
			var endDate = DateTime.Parse("2020-02-01");

			//Act
			var period = Period.FromStartAndEndDate(startDate, endDate);

			//Assert
			Assert.AreEqual(period.StartDate, startDate);
			Assert.AreEqual(period.EndDate, endDate);
		}

		[Test]
		public void FromStartAndEndDate_DefaultStartDate_ArgumentNullException()
		{
			//Arrange
			var startDate = (DateTime)default;
			var endDate = DateTime.Parse("2020-02-01");

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Period.FromStartAndEndDate(startDate, endDate));
		}

		[Test]
		public void FromStartAndEndDate_DefaultEndDate_ArgumentNullException()
		{
			//Arrange
			var startDate = DateTime.Parse("2020-02-01");
			var endDate = (DateTime)default;

			//Act & Assert
			Assert.Throws<ArgumentNullException>(() => Period.FromStartAndEndDate(startDate, endDate));
		}

		[Test]
		public void FromStartAndEndDate_EndDateEqualsToStart_PeriodWithEqualStartAndEndDate()
		{
			//Arrange
			var startDate = DateTime.Parse("2020-02-01");
			var endDate = startDate;

			//Act
			var period = Period.FromStartAndEndDate(startDate, endDate);

			//Arrange
			Assert.AreEqual(startDate, period.StartDate);
			Assert.AreEqual(endDate, period.EndDate);
		}

		[Test]
		public void FromStartAndEndDate_EndDateEarlierStart_ArgumentNullException()
		{
			//Arrange
			var startDate = DateTime.Parse("2020-02-01");
			var endDate = DateTime.Parse("2020-01-01");

			//Act & Assert
			Assert.Throws<ArgumentException>(() => Period.FromStartAndEndDate(startDate, endDate));
		}

		[Test]
		public void FromStartDateAndDuration_StardDateAndDay_Period()
		{
			//Arrange
			var startDate = DateTime.Parse("2020-01-01");
			var duration = Period.DurationEnum.Day;

			//Act
			var period = Period.FromStartDateAndDuration(startDate, duration);

			//Assert
			Assert.AreEqual(startDate, period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-01-01"), period.EndDate);
		}

		[Test]
		public void FromWeekNumber_20thWeekOf2020_ValidPeriod()
		{
			//Arrange
			var year = 2020;
			var weeknumber = 20;

			//Act
			var period = Period.FromWeekNumber(year, weeknumber);

			//Assert
			Assert.AreEqual(DateTime.Parse("2020-05-11"), period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-05-17"), period.EndDate);
		}

		[Test]
		public void FromWeekNumber_NegativeWeekOf2020_ArgumentOutOfRangeException()
		{
			//Arrange
			var year = 2020;
			var weeknumber = -1;

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => Period.FromWeekNumber(year, weeknumber));
		}

		[Test]
		public void FromWeekNumber_YearBefore1900_ArgumentOutOfRangeException()
		{
			//Arrange
			var year = 1800;
			var weeknumber = 50;

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => Period.FromWeekNumber(year, weeknumber));
		}

		[Test]
		public void FromWeekNumber_FirstWeekOf2020_ValidPeriod()
		{
			//Arrange
			var year = 2020;
			var weeknumber = 1;

			//Act
			var period = Period.FromWeekNumber(year, weeknumber);

			//Assert
			Assert.AreEqual(DateTime.Parse("2019-12-30"), period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-01-05"), period.EndDate);
		}

		[Test]
		public void FromWeekNumber_LastWeekOf2020_ValidPeriod()
		{
			//Arrange
			var year = 2020;
			var weeknumber = 53;

			//Act
			var period = Period.FromWeekNumber(year, weeknumber);

			//Assert
			Assert.AreEqual(DateTime.Parse("2020-12-28"), period.StartDate);
			Assert.AreEqual(DateTime.Parse("2021-01-03"), period.EndDate);
		}

		[Test]
		public void FromMonth_2thMonthOf2020_ValidPeriod()
		{
			//Arrange
			var year = 2020;
			var month = 2;

			//Act
			var period = Period.FromMonth(year, month);

			//Assert
			Assert.AreEqual(DateTime.Parse("2020-02-01"), period.StartDate);
			Assert.AreEqual(DateTime.Parse("2020-02-29"), period.EndDate);
		}

		[Test]
		public void FromMonth_NegativeMonthOf2020_ArgumentOutOfRangeException()
		{
			//Arrange
			var year = 2020;
			var month = -1;

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => Period.FromMonth(year, month));
		}

		[Test]
		public void FromMonth_13thMonthOf2020_ArgumentOutOfRangeException()
		{
			//Arrange
			var year = 2020;
			var month = 13;

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => Period.FromMonth(year, month));
		}

		[Test]
		public void FromMonth_YearBefore1900_ArgumentOutOfRangeException()
		{
			//Arrange
			var year = 1800;
			var month = 2;

			//Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => Period.FromMonth(year, month));
		}
	}
}

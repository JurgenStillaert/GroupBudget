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
	}
}

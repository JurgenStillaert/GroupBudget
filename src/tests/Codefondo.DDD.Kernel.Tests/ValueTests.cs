using Codefondo.DDD.Kernel.Tests.Values;
using NUnit.Framework;

namespace Codefondo.DDD.Kernel.Tests
{
	public class ValueTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void StringValues_Equal_EqualValues_True()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Test");

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void StringValues_Equal_DifferentValues_False()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Other");

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void StringValues_EqualOperator_EqualParameters_True()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Test");

			//Act
			var result = value1 == value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void StringValues_EqualOperator_InequalParameters_False()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Test");

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void StringValues_InequalOperator_InequalParameters_True()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Other");

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void StringValues_InequalOperator_EqualParameters_False()
		{
			//Arrange
			var value1 = StringValue.FromString("Test");
			var value2 = StringValue.FromString("Test");

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void StringValues_ToString_StringValue_Equals()
		{
			//Arrange
			var origString = "Test";
			var value = StringValue.FromString(origString);

			//Assert
			Assert.AreEqual(origString, value.ToString());
		}

		[Test]
		public void Equal_intValues_Equals_True()
		{
			//Arrange
			var value1 = IntValue.FromInt(5);
			var value2 = IntValue.FromInt(5);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsTrue(result);
		}
	}
}
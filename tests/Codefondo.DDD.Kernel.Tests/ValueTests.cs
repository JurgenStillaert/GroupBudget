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
		public void Equal_StringValues_Equals_True()
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
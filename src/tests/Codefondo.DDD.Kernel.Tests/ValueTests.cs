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

		#region StringValues

		[Test]
		public void StringValues_Equals_EqualValues_True()
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
		public void StringValues_Equals_DifferentValues_False()
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
			const string origString = "Test";
			var value = StringValue.FromString(origString);

			//Assert
			Assert.AreEqual(origString, value.ToString());
		}

		#endregion StringValues

		#region IntValues

		[Test]
		public void IntValues_Equals_EqualValues_True()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(1);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void IntValues_Equals_DifferentValues_False()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(2);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void IntValues_EqualOperator_EqualParameters_True()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(1);

			//Act
			var result = value1 == value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void IntValues_EqualOperator_InequalParameters_False()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(1);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void IntValues_InequalOperator_InequalParameters_True()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(2);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void IntValues_InequalOperator_EqualParameters_False()
		{
			//Arrange
			var value1 = IntValue.FromInt(1);
			var value2 = IntValue.FromInt(1);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void IntValues_ToString_IntValue_Equals()
		{
			//Arrange
			const int origInt = 1;
			var value = IntValue.FromInt(origInt);

			//Assert
			Assert.AreEqual(origInt.ToString(), value.ToString());
		}

		#endregion IntValues

		#region EnumValues

		[Test]
		public void EnumValues_Equals_EqualValues_True()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum1);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void EnumValues_Equals_DifferentValues_False()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum2);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void EnumValues_EqualOperator_EqualParameters_True()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum1);

			//Act
			var result = value1 == value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void EnumValues_EqualOperator_InequalParameters_False()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum1);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void EnumValues_InequalOperator_InequalParameters_True()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum2);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void EnumValues_InequalOperator_EqualParameters_False()
		{
			//Arrange
			var value1 = EnumValue.FromEnum(TestEnum.enum1);
			var value2 = EnumValue.FromEnum(TestEnum.enum1);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void EnumValues_ToString_EnumValue_Equals()
		{
			//Arrange
			const Codefondo.DDD.Kernel.Tests.Values.TestEnum origEnum = TestEnum.enum1;
			var value = EnumValue.FromEnum(origEnum);

			//Assert
			Assert.AreEqual(origEnum.ToString(), value.ToString());
		}

		#endregion EnumValues

		#region BoolValues

		[Test]
		public void BoolValues_Equals_EqualValues_True()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(true);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void BoolValues_Equals_DifferentValues_False()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(false);

			//Act
			var result = value1.Equals(value2);

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void BoolValues_EqualOperator_EqualParameters_True()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(true);

			//Act
			var result = value1 == value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void BoolValues_EqualOperator_InequalParameters_False()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(true);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void BoolValues_InequalOperator_InequalParameters_True()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(false);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void BoolValues_InequalOperator_EqualParameters_False()
		{
			//Arrange
			var value1 = BoolValue.FromBool(true);
			var value2 = BoolValue.FromBool(true);

			//Act
			var result = value1 != value2;

			//Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void BoolValues_ToString_BoolValue_Equals()
		{
			//Arrange
			const bool origBool = true;
			var value = BoolValue.FromBool(origBool);

			//Assert
			Assert.AreEqual(origBool.ToString(), value.ToString());
		}

		#endregion BoolValues
	}
}
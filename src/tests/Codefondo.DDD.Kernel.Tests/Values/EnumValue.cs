namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class EnumValue : Value<EnumValue>
	{
		internal EnumValue(TestEnum value) => Value = value;

		protected EnumValue()
		{
		}

		public TestEnum Value { get; }

		public static EnumValue FromEnum(TestEnum i)
		{
			return new EnumValue(i);
		}

		public static implicit operator TestEnum(EnumValue i) => i.Value;
	}

	public enum TestEnum
	{
		enum1,
		enum2
	}
}
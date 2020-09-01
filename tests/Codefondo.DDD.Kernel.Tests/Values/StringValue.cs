namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class StringValue : Value<StringValue>
	{
		internal StringValue(string value) => Value = value;

		protected StringValue()
		{
		}

		public string Value { get; }

		public static StringValue FromString(string str)
		{
			return new StringValue(str);
		}

		public static implicit operator string(StringValue str) => str.Value;
	}
}
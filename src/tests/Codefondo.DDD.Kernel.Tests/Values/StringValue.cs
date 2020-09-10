namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class StringValue : Value<StringValue>
	{
#pragma warning disable CC0057 // Unused parameters
		internal StringValue(string value) => Value = value;
#pragma warning restore CC0057 // Unused parameters

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
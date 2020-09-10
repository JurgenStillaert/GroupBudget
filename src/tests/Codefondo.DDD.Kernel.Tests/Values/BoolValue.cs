namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class BoolValue : Value<BoolValue>
	{
#pragma warning disable CC0057 // Unused parameters
		internal BoolValue(bool value) => Value = value;
#pragma warning restore CC0057 // Unused parameters

		protected BoolValue()
		{
		}

		public bool Value { get; }

		public static BoolValue FromBool(bool i)
		{
			return new BoolValue(i);
		}

		public static implicit operator bool(BoolValue i) => i.Value;
	}
}
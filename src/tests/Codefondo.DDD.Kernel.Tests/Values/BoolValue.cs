namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class BoolValue : Value<BoolValue>
	{
		internal BoolValue(bool value) => Value = value;

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
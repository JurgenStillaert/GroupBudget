namespace Codefondo.DDD.Kernel.Tests.Values
{
	public class IntValue : Value<IntValue>
	{
		internal IntValue(int value) => Value = value;

		protected IntValue()
		{
		}

		public int Value { get; }

		public static IntValue FromInt(int i)
		{
			return new IntValue(i);
		}

		public static implicit operator int(IntValue i) => i.Value;
	}
}
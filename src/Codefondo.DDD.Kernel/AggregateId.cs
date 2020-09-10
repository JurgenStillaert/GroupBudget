using System;

namespace Codefondo.DDD.Kernel
{
	public abstract class AggregateId<T> : Value<AggregateId<T>>
		where T : AggregateRoot
	{
		public Guid Value { get; }

		protected AggregateId(Guid value)
		{
			if (value == default)
			{
				throw new ArgumentNullException("The aggregate ID cannot be empty", nameof(value));
			}

			Value = value;
		}

		public static implicit operator Guid(AggregateId<T> self) => self.Value;

		public override string ToString() => Value.ToString();
	}
}
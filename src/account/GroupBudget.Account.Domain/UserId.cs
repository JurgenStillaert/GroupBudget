using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class UserId : Value<UserId>
	{
		public Guid Value { get; }

		protected UserId(Guid value)
		{
			if (value == default)
			{
				throw new ArgumentNullException(nameof(value), "The user id cannot be empty");
			}

			Value = value;
		}

		public static UserId FromGuid(Guid value) => new UserId(value);

		public static implicit operator Guid(UserId self) => self.Value;

		public override string ToString() => Value.ToString();
	}
}
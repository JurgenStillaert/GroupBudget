using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.SharedKernel
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
		public static UserId FromString(string ownerId) => new UserId(Guid.Parse(ownerId));


		public static implicit operator Guid(UserId self) => self.Value;

	}
}
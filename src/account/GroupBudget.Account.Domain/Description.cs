using Codefondo.DDD.Kernel;
using System;

namespace GroupBudget.Account.Domain
{
	public class Description : Value<Description>
	{
		public string Value { get; }

		protected Description(string value)
		{
			if (value.Length < 2)
				throw new ArgumentException("Description must have at least 2 charachters", nameof(value));

			if (value.Length > 30)
				throw new ArgumentException("Description must have a maximum of 30 charachters", nameof(value));

			Value = value;
		}

		public static Description FromString(string description)
			=> new Description(description);
	}
}
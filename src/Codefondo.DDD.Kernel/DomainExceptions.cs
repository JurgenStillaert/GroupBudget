using System;

namespace Codefondo.DDD.Kernel
{
	public static class DomainExceptions
	{
#pragma warning disable CA1034 // Nested types should not be visible
		public class InvalidEntityState : Exception
		{
			public InvalidEntityState(object entity, string message, Exception innerException = null)
				: base($"Entity {entity.GetType().Name} state change rejected, { message}", innerException)
			{
			}

			public InvalidEntityState()
			{
			}

			public InvalidEntityState(string message) : base(message)
			{
			}

			public InvalidEntityState(string message, Exception innerException) : base(message, innerException)
			{
			}
		}
#pragma warning restore CA1034 // Nested types should not be visible
	}
}
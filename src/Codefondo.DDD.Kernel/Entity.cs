using System;
using System.Reflection;

namespace Codefondo.DDD.Kernel
{
	public abstract class Entity<TId>
		where TId : Value<TId>
	{
		public TId Id { get; protected set; }

		/// <summary>
		/// Find Handle methods in the implementation with parameter of type @event
		/// </summary>
		/// <param name="event"></param>
		protected internal void When(IDomainEvent @event)
		{
			//Get the handle methods
			var handleMethod = this.GetType().GetMethod(
					"Handle",
					BindingFlags.Instance | BindingFlags.NonPublic,
					Type.DefaultBinder,
					new Type[] { @event.GetType() },
					null);

			if (handleMethod == null)
			{
				throw new MissingMethodException($"Handle method with event { @event.GetType()} is missing");
			}

			handleMethod.Invoke(this, new object[] { @event });
		}

		protected void Apply(IDomainEvent @event)
		{
			When(@event);
			EnsureValidation();
		}

		protected internal abstract void EnsureValidation();
	}
}
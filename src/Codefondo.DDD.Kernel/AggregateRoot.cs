using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codefondo.DDD.Kernel
{
	public abstract class AggregateRoot : IInternalEventHandler
	{
		public Guid Id { get; protected set; }
		public int Version { get; private set; } = -1;

		public IReadOnlyList<IDomainEvent> GetChanges() => _changes.ToList();

		public void ClearChanges() => _changes.Clear();

		void IInternalEventHandler.Handle(IDomainEvent @event) => When(@event);

		/// <summary>
		/// Find Handle methods in the implementation with parameter of type @event
		/// </summary>
		/// <param name="event"></param>
		protected void When(object @event)
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
			_changes.Add(@event);
		}

		protected abstract void EnsureValidation();

#pragma warning disable CC0091 // Use static method
#pragma warning disable CA1822 // Mark members as static
		protected void ApplyToEntity(IInternalEventHandler entity, IDomainEvent @event) => entity?.Handle(@event);
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore CC0091 // Use static method

		private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();
	}
}
using System;

namespace Codefondo.DDD.Kernel
{
	public abstract class Entity<TId> : IInternalEventHandler
		where TId : Value<TId>
	{
		readonly Action<object> _applier;

		public TId Id { get; protected set; }

#pragma warning disable CC0057 // Unused parameters
		protected Entity(Action<object> applier) => _applier = applier;
#pragma warning restore CC0057 // Unused parameters

		public void Handle(IDomainEvent @event) => When(@event);

		protected abstract void When(object @event);

		protected void Apply(object @event)
		{
			When(@event);
			_applier(@event);
		}
	}
}
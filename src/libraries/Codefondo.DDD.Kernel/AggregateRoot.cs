using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Codefondo.DDD.Kernel.DomainExceptions;

namespace Codefondo.DDD.Kernel
{
	public abstract class AggregateRoot
	{
		public Guid Id { get; protected set; }

		public IReadOnlyList<IDomainEvent> GetChanges() => _changes.ToList();

		public void ClearChanges() => _changes.Clear();

		/// <summary>
		/// Find Handle methods in the implementation with parameter of type @event
		/// </summary>
		/// <param name="event"></param>
		protected void When(IDomainEvent @event)
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

			try
			{
				handleMethod.Invoke(this, new object[] { @event });
			}
			catch (TargetInvocationException targetInvocationException)
			{
				throw targetInvocationException.InnerException;
			}
		}

		protected void Apply(IDomainEvent @event)
		{
			When(@event);
			EnsureValidation();
			_changes.Add(@event);
		}

		public void Replay(List<IDomainEvent> history)
		{
			foreach (var @event in history)
			{
				When(@event);
			}
		}

		protected abstract void EnsureValidation();

		private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();

		protected static void ApplyToEntity(IEntity entity, IDomainEvent @event) => entity?.Apply(@event);
	}
}
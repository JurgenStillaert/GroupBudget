namespace Codefondo.DDD.Kernel
{
	public interface IEntity
	{
		void Apply(IDomainEvent @event);
	}
}
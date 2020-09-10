namespace Codefondo.DDD.Kernel
{
	public interface IInternalEventHandler
	{
		void Handle(IDomainEvent @event);
	}
}
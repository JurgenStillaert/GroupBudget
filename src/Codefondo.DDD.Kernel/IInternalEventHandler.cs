namespace Codefondo.DDD.Kernel
{
	public interface IInternalEventHandler
	{
		void Handle(object @event);
	}
}
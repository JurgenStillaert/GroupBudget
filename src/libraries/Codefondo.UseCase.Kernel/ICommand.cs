using System;
using System.Threading.Tasks;

namespace Codefondo.UseCase.Kernel
{
	public interface ICommand
	{
	}

	public interface ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		Task Apply(TCommand command);
	}
}
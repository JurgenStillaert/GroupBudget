using Codefondo.DDD.Kernel;
using System;
using System.Threading.Tasks;

namespace Codefondo.UseCase.Kernel
{
	public interface IRepository<TAggregate>
		 where TAggregate : AggregateRoot
	{
		Task Save(TAggregate aggregateRoot);
		Task<AggregateRoot> Load(Guid aggregateId);
	}
}
using Codefondo.DDD.Kernel;
using MediatR;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Codefondo.UseCase.Kernel
{
	public abstract class CreateCommandHandler<TCommand, TAggregate> : CommandHandler<TCommand, TAggregate>
		where TAggregate : AggregateRoot
		where TCommand : IRequest
	{
		protected CreateCommandHandler(IRepository<TAggregate> repo, IMediator mediator)
			: base(repo, CommandHandler<TCommand, TAggregate>.HandlerTypeEnum.Create, mediator)
		{
		}
	}

	public abstract class UpdateCommandHandler<TCommand, TAggregate> : CommandHandler<TCommand, TAggregate>
		where TAggregate : AggregateRoot
		where TCommand : IRequest
	{
		protected UpdateCommandHandler(IRepository<TAggregate> repo, IMediator mediator)
			: base(repo, CommandHandler<TCommand, TAggregate>.HandlerTypeEnum.Update, mediator)
		{ }

		protected abstract void PreHandle(TCommand command);
	}

	public abstract class CommandHandler<TCommand, TAggregate> : IRequestHandler<TCommand>
		where TAggregate : AggregateRoot
		where TCommand : IRequest
	{
		protected CommandHandler(IRepository<TAggregate> repo, HandlerTypeEnum handlerType, IMediator mediator)
		{
			Repo = repo;
			CommandType = handlerType;
			this.mediator = mediator;
		}

		public IRepository<TAggregate> Repo { get; }
		public AggregateId<TAggregate> AggregateId { get; protected set; }
		public TAggregate AggregateRoot { get; protected set; }
		private HandlerTypeEnum CommandType { get; }
		protected readonly IMediator mediator;

		public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
		{
			if (CommandType == HandlerTypeEnum.Update)
			{
				//Execute prehandle method to get aggregateId
				var preHandleMethod = GetHandleMethod(command, HandleMethodType.PreHandle);

				try
				{
					preHandleMethod.Invoke(this, new object[] { command });
				}
				catch (TargetInvocationException targetInvocationException)
				{
					throw targetInvocationException.InnerException;
				}
				catch (Exception)
				{
					throw;
				}

				AggregateRoot = await GetAggregateFromRepo();
			}

			try
			{
				var aggregateRoot = await Apply(command, cancellationToken);

				if (aggregateRoot != null)
				{
					await Repo.Save(aggregateRoot);

					//Public events
					foreach (var @event in aggregateRoot.GetChanges())
					{
						await mediator.Publish(@event, cancellationToken);
					}
				}
			}
			catch (TargetInvocationException targetInvocationException)
			{
				throw targetInvocationException.InnerException;
			}
			catch (Exception)
			{
				throw;
			}

			return await Unit.Task;
		}

		protected async Task<TAggregate> GetAggregateFromRepo()
		{
			return (TAggregate)await Repo.Load(AggregateId.Value);
		}

		protected abstract Task<TAggregate> Apply(TCommand command, CancellationToken cancellationToken);

		private MethodInfo GetHandleMethod(TCommand command, HandleMethodType handleMethodType)
		{
			//Get the handle methods
			var handleMethod = this.GetType().GetMethod(
					Enum.GetName(typeof(HandleMethodType), handleMethodType),
					BindingFlags.Instance | BindingFlags.NonPublic,
					Type.DefaultBinder,
					new Type[] { command.GetType() },
					null);

			if (handleMethod == null)
			{
				throw new MissingMethodException($"Handle method with event { command.GetType()} is missing");
			}

			return handleMethod;
		}

		private enum HandleMethodType
		{
			Handle,
			PreHandle
		}

		protected enum HandlerTypeEnum
		{
			Update,
			Create
		}
	}
}
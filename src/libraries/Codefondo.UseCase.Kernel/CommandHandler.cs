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
		where TCommand : IRequest<Unit>
	{
		protected CreateCommandHandler(IRepository<TAggregate> repo, IMediator mediator)
			: base(repo, CommandHandler<TCommand, TAggregate>.HandlerTypeEnum.Create, mediator)
		{
		}
	}

	public abstract class UpdateCommandHandler<TCommand, TAggregate> : CommandHandler<TCommand, TAggregate>
		where TAggregate : AggregateRoot
		where TCommand : IRequest<Unit>
	{
		protected UpdateCommandHandler(IRepository<TAggregate> repo, IMediator mediator)
			: base(repo, CommandHandler<TCommand, TAggregate>.HandlerTypeEnum.Update, mediator)
		{
		}
	}

	public abstract class CommandHandler<TCommand, TAggregate> : IRequestHandler<TCommand>
		where TAggregate : AggregateRoot
		where TCommand : IRequest<Unit>
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
		private readonly IMediator mediator;

		public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
		{
			//Execute prehandle method to get aggregateId
			var preHandleMethod = GetHandleMethod(request, HandleMethodType.PreHandle);

			if (CommandType == HandlerTypeEnum.Update)
			{
				AggregateRoot = (TAggregate)await Repo.Load(AggregateId.Value);
			}

			try
			{
				preHandleMethod.Invoke(this, new object[] { request });
			}
			catch (TargetInvocationException targetInvocationException)
			{
				throw targetInvocationException.InnerException;
			}
			catch (Exception)
			{
				throw;
			}

			var handleMethod = GetHandleMethod(request, HandleMethodType.Handle);

			try
			{
				var aggregateRoot = (TAggregate)handleMethod.Invoke(this, new object[] { request });
				await Repo.Save(aggregateRoot);

				//Public events
				foreach (var @event in aggregateRoot.GetChanges())
				{
					await mediator.Publish(@event, cancellationToken);
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
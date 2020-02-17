using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Rebus;
using Rebus.Bus;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseRebusStage : IStage, IStageAsync
    {
        protected IBus Bus { get; }

        protected BaseRebusStage(IBus bus)
        {
            Bus = bus;
        }

        protected abstract bool IsAsync(IPipelineContext context);

        protected virtual void Send(IMessage message, IPipelineContext context)
        {
            Bus.Advanced.SyncBus.Send(message);
        }

        protected virtual Task SendAsync(IMessage message, IPipelineContext context, CancellationToken cancellationToken)
        {
            return Bus.Send(message);
        }

        protected virtual void Publish(IMessage message, IPipelineContext context)
        {
            Bus.Advanced.SyncBus.Publish(message);
        }

        protected virtual Task PublishAsync(IMessage message, IPipelineContext context, CancellationToken cancellationToken)
        {
            return Bus.Publish(message);
        }

        protected virtual void Handle(IMessage message, global::Rebus.Pipeline.IMessageContext rebusContext, IPipelineContext pipelineContext)
        {
        }

        protected virtual Task HandleAsync(IMessage message, global::Rebus.Pipeline.IMessageContext rebusContext, IPipelineContext pipelineContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public virtual void Execute(IPipelineContext context, Action next)
        {
            var rebusContext = RebusContext.Current;

            if (IsAsync(context) && rebusContext == null)
            {
                if (context is EventContext)
                {
                    Publish(context.Message, context);
                }
                else
                {
                    Send(context.Message, context);
                }
            }
            else
            {
                using (RebusContext.Assign(null))
                {
                    if (rebusContext != null)
                    {
                        Handle(context.Message, rebusContext, context);
                    }

                    next();
                }
            }
        }

        public virtual async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var rebusContext = RebusContext.Current;

            if (IsAsync(context) && rebusContext == null)
            {
                if (context is EventContext)
                {
                    await PublishAsync(context.Message, context, cancellationToken);
                }
                else
                {
                    await SendAsync(context.Message, context, cancellationToken);
                }
            }
            else
            {
                using (RebusContext.Assign(null))
                {
                    if (rebusContext != null)
                    {
                        await HandleAsync(context.Message, rebusContext, context, cancellationToken);
                    }

                    await next();
                }
            }
        }
    }
}
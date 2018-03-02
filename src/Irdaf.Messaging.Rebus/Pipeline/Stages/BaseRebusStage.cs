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

        protected virtual void Send(IPipelineContext context)
        {
            Bus.Advanced.SyncBus.Send(context.Message);
        }

        protected virtual Task SendAsync(IPipelineContext context)
        {
            return Bus.Send(context.Message);
        }

        protected virtual void Publish(IPipelineContext context)
        {
            Bus.Advanced.SyncBus.Publish(context.Message);
        }

        protected virtual Task PublishAsync(IPipelineContext context)
        {
            return Bus.Publish(context.Message);
        }

        protected virtual void Handle(global::Rebus.Pipeline.IMessageContext rebusContext, IPipelineContext pipelineContext)
        {
        }

        protected virtual Task HandleAsync(global::Rebus.Pipeline.IMessageContext rebusContext, IPipelineContext pipelineContext)
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
                    Publish(context);
                }
                else
                {
                    Send(context);
                }
            }
            else
            {
                using (RebusContext.Assing(null))
                {
                    if (rebusContext != null)
                    {
                        Handle(rebusContext, context);
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
                    await PublishAsync(context);
                }
                else
                {
                    await SendAsync(context);
                }
            }
            else
            {
                using (RebusContext.Assing(null))
                {
                    if (rebusContext != null)
                    {
                        await HandleAsync(rebusContext, context);
                    }

                    await next();
                }
            }
        }
    }
}
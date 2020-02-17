using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Rebus;
using Rebus.Bus;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseRebusStage<TDeferMessage> : BaseRebusStage
    {
        protected BaseRebusStage(IBus bus)
            : base(bus)
        {
        }

        protected abstract TimeSpan GetDelay(TDeferMessage message, IPipelineContext context);

        protected abstract IMessage GetMessage(TDeferMessage message, IPipelineContext context);

        protected virtual void Defer(TimeSpan delay, IMessage message, IPipelineContext context)
        {
            Bus.Advanced.SyncBus.Defer(delay, message);
        }

        protected virtual Task DeferAsync(TimeSpan delay, IMessage message, IPipelineContext context, CancellationToken cancellationToken)
        {
            return Bus.Defer(delay, message);
        }

        public override void Execute(IPipelineContext context, Action next)
        {
            if (context.Message is TDeferMessage defer && RebusContext.Current == null)
            {
                Defer(GetDelay(defer, context), GetMessage(defer, context), context);
            }
            else
            {
                base.Execute(context, next);
            }
        }

        public override async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            if (context.Message is TDeferMessage defer && RebusContext.Current == null)
            {
                await DeferAsync(GetDelay(defer, context), GetMessage(defer, context), context, cancellationToken);
            }
            else
            {
                await base.ExecuteAsync(context, next, cancellationToken);
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class InvokeHandlerStage : IStage, IStageAsync
    {
        public void Execute(IPipelineContext context, Action next)
        {
            var handlers = context.Get<HandlerList>();
            var convention = context.Get<IMessageConvention>();

            convention.Invoke(handlers, context);

            next();
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var handlers = context.Get<HandlerList>();
            var convention = context.Get<IMessageConvention>();

            await convention.InvokeAsync(handlers, context, cancellationToken);

            await next();
        }
    }
}
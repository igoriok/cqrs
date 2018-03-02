using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class InvokeHandlerStage : IStage, IStageAsync
    {
        public void Execute(IPipelineContext context, Action next)
        {
            var handlers = context.Get<HandlerList>();

            context.Convention.Invoke(handlers, context);

            next();
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var handlers = context.Get<HandlerList>();

            await context.Convention.InvokeAsync(handlers, context, cancellationToken);

            await next();
        }
    }
}
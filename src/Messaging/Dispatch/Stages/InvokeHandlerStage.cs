using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Dispatch.Executors;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Dispatch.Stages
{
    public class InvokeHandlerStage : IStage
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public InvokeHandlerStage(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void Execute(IMessageContext context, Action next)
        {
            var handlers = _handlerRegistry.GetHandlers(context);
            var executor = context.Get<IExecutor>();

            executor.Execute(handlers);

            next();
        }

        public async Task ExecuteAsync(IMessageContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var handlers = _handlerRegistry.GetHandlers(context);
            var executor = context.Get<IExecutor>();

            await executor.ExecuteAsync(handlers, cancellationToken);

            await next();
        }
    }
}
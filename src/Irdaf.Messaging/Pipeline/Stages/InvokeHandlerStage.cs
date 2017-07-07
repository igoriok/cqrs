using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class InvokeHandlerStage : IStage
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public InvokeHandlerStage(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            var executor = (IExecutor)context;

            executor.Execute(_handlerRegistry);

            next();
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var executor = (IExecutor)context;

            await executor.ExecuteAsync(_handlerRegistry, cancellationToken);

            await next();
        }
    }
}
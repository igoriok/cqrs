using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class ResolveHandlerStage : IStage, IStageAsync
    {
        private readonly IHandlerRegistry _handlerRegistry;

        public ResolveHandlerStage(IHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            var syncHandlers = GetHandlers(context);
            var asyncHandlers = GetHandlersAsync(context);

            var handlerList = new HandlerList(syncHandlers.Concat(asyncHandlers));

            context.Set(handlerList);

            next();
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var syncHandlers = GetHandlers(context);
            var asyncHandlers = GetHandlersAsync(context);

            var handlerList = new HandlerList(asyncHandlers.Concat(syncHandlers).Distinct());

            context.Set(handlerList);

            await next();
        }

        private IEnumerable<object> GetHandlers(IPipelineContext context)
        {
            var handlerType = context.Get<IMessageConvention>().GetHandlerType();

            return _handlerRegistry.GetHandlers(handlerType, context).OfType<object>();
        }

        private IEnumerable<object> GetHandlersAsync(IPipelineContext context)
        {
            var handlerType = context.Get<IMessageConvention>().GetHandlerAsyncType();

            return _handlerRegistry.GetHandlers(handlerType, context).OfType<object>();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public class AutofacMessageHandlerProviderAsync : MessageHandlerProviderAsync
    {
        private readonly IComponentContext _context;

        public AutofacMessageHandlerProviderAsync(IComponentContext context)
        {
            _context = context;
        }

        public override async Task<IQueryHandlerAsync<IQuery<TResult>, TResult>> GetQueryHandlerAsync<TResult>(Type queryType, CancellationToken token)
        {
            var resultType = typeof(TResult);
            var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(queryType, resultType);

            object handler;

            if (_context.TryResolve(handlerType, out handler))
            {
                return WrapQueryHandler<TResult>(queryType, _context.Resolve(handlerType));
            }

            return await base.GetQueryHandlerAsync<TResult>(queryType, token);
        }

        public override async Task<ICommandHandlerAsync<ICommand>> GetCommandHandlerAsync(Type commandType, CancellationToken token)
        {
            var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(commandType);

            object handler;

            if (_context.TryResolve(handlerType, out handler))
            {
                return WrapCommandHandler(commandType, _context.Resolve(handlerType));
            }

            return await base.GetCommandHandlerAsync(commandType, token);
        }

        public override async Task<IEnumerable<IEventHandlerAsync<IEvent>>> GetEventHandlersAsync(Type eventType, CancellationToken token)
        {
            var handlerType = typeof(IEventHandlerAsync<>).MakeGenericType(eventType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

            return ((IEnumerable)_context.Resolve(enumerableType))
                .Cast<object>()
                .Select(handler => WrapEventHandler(eventType, handler))
                .Concat(await base.GetEventHandlersAsync(eventType, token));
        }
    }
}
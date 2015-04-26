using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public class AutofacMessageHandlerProvider : MessageHandlerProvider
    {
        private readonly IComponentContext _context;

        public AutofacMessageHandlerProvider(IComponentContext context)
        {
            _context = context;
        }

        public override IQueryHandler<IQuery<TResult>, TResult> GetQueryHandler<TResult>(Type queryType)
        {
            var resultType = typeof(TResult);
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);

            object handler;

            if (_context.TryResolve(handlerType, out handler))
            {
                return WrapQueryHandler<TResult>(queryType, _context.Resolve(handlerType));
            }

            return base.GetQueryHandler<TResult>(queryType);
        }

        public override ICommandHandler<ICommand> GetCommandHandler(Type commandType)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            return WrapCommandHandler(commandType, _context.Resolve(handlerType));
        }

        public override IEnumerable<IEventHandler<IEvent>> GetEventHandlers(Type eventType)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

            return ((IEnumerable)_context
                .Resolve(enumerableType))
                .Cast<IEventHandler<IEvent>>()
                .Select(h => WrapEventHandler(eventType, h));
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Handlers
{
    public class DefaultHandlerSource : IHandlerSource
    {
        private readonly Dictionary<Type, IList<Func<object>>> _handlers = new Dictionary<Type, IList<Func<object>>>();

        public void RegisterCommandHandler<TCommand>(ICommandHandler<TCommand> handler)
            where TCommand : ICommand
        {
            AddHandler(typeof(ICommandHandler<TCommand>), () => handler);
        }

        public void RegisterCommandHandler<TCommand, TCommandHandler>()
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>, new()
        {
            AddHandler(typeof(ICommandHandler<TCommand>), () => new TCommandHandler());
        }

        public void RegisterCommandHandlerAsync<TCommand>(ICommandHandlerAsync<TCommand> handler)
            where TCommand : ICommand
        {
            AddHandler(typeof(ICommandHandlerAsync<TCommand>), () => handler);
        }

        public void RegisterCommandHandlerAsync<TCommand, TCommandHandlerAsync>()
            where TCommand : ICommand
            where TCommandHandlerAsync : ICommandHandlerAsync<TCommand>, new()
        {
            AddHandler(typeof(ICommandHandlerAsync<TCommand>), () => new TCommandHandlerAsync());
        }

        public void RegisterQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
            where TQuery : IQuery<TResult>
        {
            AddHandler(typeof(IQueryHandler<TQuery, TResult>), () => handler);
        }

        public void RegisterQueryHandler<TQuery, TResult, TQueryHandler>()
            where TQuery : IQuery<TResult>
            where TQueryHandler : IQueryHandler<TQuery, TResult>, new()
        {
            AddHandler(typeof(IQueryHandler<TQuery, TResult>), () => new TQueryHandler());
        }

        public void RegisterQueryHandlerAsync<TQuery, TResult>(IQueryHandlerAsync<TQuery, TResult> handler)
            where TQuery : IQuery<TResult>
        {
            AddHandler(typeof(IQueryHandlerAsync<TQuery, TResult>), () => handler);
        }

        public void RegisterQueryHandlerAsync<TQuery, TResult, TQueryHandlerAsync>()
            where TQuery : IQuery<TResult>
            where TQueryHandlerAsync : IQueryHandler<TQuery, TResult>, new()
        {
            AddHandler(typeof(IQueryHandlerAsync<TQuery, TResult>), () => new TQueryHandlerAsync());
        }

        public void RegisterEventHandler<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent
        {
            AddHandler(typeof(IEventHandler<TEvent>), () => handler);
        }

        public void RegisterEventHandler<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>, new()
        {
            AddHandler(typeof(IEventHandler<TEvent>), () => new TEventHandler());
        }

        public void RegisterEventHandlerAsync<TEvent>(IEventHandlerAsync<TEvent> handler)
            where TEvent : IEvent
        {
            AddHandler(typeof(IEventHandlerAsync<TEvent>), () => handler);
        }

        public void RegisterEventHandlerAsync<TEvent, TEventHandlerAsync>()
            where TEvent : IEvent
            where TEventHandlerAsync : IEventHandlerAsync<TEvent>, new()
        {
            AddHandler(typeof(IEventHandlerAsync<TEvent>), () => new TEventHandlerAsync());
        }

        public IEnumerable GetHandlers(Type handlerType, IPipelineContext context)
        {
            if (_handlers.TryGetValue(handlerType, out var handlers))
            {
                return handlers.Select(h => h());
            }

            return Enumerable.Empty<object>();
        }

        private void AddHandler(Type handlerType, Func<object> handler)
        {
            if (!_handlers.TryGetValue(handlerType, out var handlers))
            {
                handlers = new List<Func<object>>();

                _handlers[handlerType] = handlers;
            }

            handlers.Add(handler);
        }
    }
}
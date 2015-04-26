using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public class MessageHandlerProviderAsync : IMessageHandlerProviderAsync, IDisposable
    {
        private readonly ConcurrentDictionary<Type, ICollection<object>> _dictionary;

        public MessageHandlerProviderAsync()
        {
            _dictionary = new ConcurrentDictionary<Type, ICollection<object>>();
        }

        public void AddQueryHandlerAsync<TQuery, TResult>(IQueryHandlerAsync<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var collection = GetHandlersCollection(typeof(TQuery));

            collection.Add(handler);
        }

        public void RemoveQueryHandlerAsync<TQuery, TResult>(IQueryHandlerAsync<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var collection = GetHandlersCollection(typeof(TQuery));

            collection.Remove(handler);
        }

        public virtual Task<IQueryHandlerAsync<IQuery<TResult>, TResult>> GetQueryHandlerAsync<TResult>(Type queryType, CancellationToken token)
        {
            return Task.FromResult(GetHandlersCollection(queryType).Select(handler => WrapQueryHandler<TResult>(queryType, handler)).Single());
        }

        public void AddCommandHandlerAsync<TCommand>(ICommandHandlerAsync<TCommand> handler) where TCommand : ICommand
        {
            var collection = GetHandlersCollection(typeof(TCommand));

            collection.Add(handler);
        }

        public void RemoveCommandHandlerAsync<TCommand>(ICommandHandlerAsync<TCommand> handler) where TCommand : ICommand
        {
            var collection = GetHandlersCollection(typeof(TCommand));

            collection.Remove(handler);
        }

        public virtual Task<ICommandHandlerAsync<ICommand>> GetCommandHandlerAsync(Type commandType, CancellationToken token)
        {
            return Task.FromResult(GetHandlersCollection(commandType).Select(handler => WrapCommandHandler(commandType, handler)).Single());
        }

        public void AddEventHandlerAsync<TEvent>(IEventHandlerAsync<TEvent> handler) where TEvent : IEvent
        {
            var collection = GetHandlersCollection(typeof(TEvent));

            collection.Add(handler);
        }

        public void RemoveEventHandlerAsync<TEvent>(IEventHandlerAsync<TEvent> handler) where TEvent : IEvent
        {
            var collection = GetHandlersCollection(typeof(TEvent));

            collection.Remove(handler);
        }

        public virtual Task<IEnumerable<IEventHandlerAsync<IEvent>>> GetEventHandlersAsync(Type eventType, CancellationToken token)
        {
            return Task.FromResult(GetHandlersCollection(eventType).Select(handler => WrapEventHandler(eventType, handler)));
        }

        public void Dispose()
        {
            _dictionary.Clear();
        }

        protected IQueryHandlerAsync<IQuery<TResult>, TResult> WrapQueryHandler<TResult>(Type queryType, object handler)
        {
            var adapterType = typeof(QueryHandlerAdapterAsync<,>).MakeGenericType(queryType, typeof(TResult));

            return (IQueryHandlerAsync<IQuery<TResult>, TResult>)Activator.CreateInstance(adapterType, handler);
        }

        protected ICommandHandlerAsync<ICommand> WrapCommandHandler(Type eventType, object handler)
        {
            var adapterType = typeof(CommandHandlerAdapterAsync<>).MakeGenericType(eventType);

            return (ICommandHandlerAsync<ICommand>)Activator.CreateInstance(adapterType, handler);
        }

        protected IEventHandlerAsync<IEvent> WrapEventHandler(Type eventType, object handler)
        {
            var adapterType = typeof(EventHandlerAdapterAsync<>).MakeGenericType(eventType);

            return (IEventHandlerAsync<IEvent>)Activator.CreateInstance(adapterType, handler);
        }

        private ICollection<object> GetHandlersCollection(Type eventType)
        {
            ICollection<object> collection = new SynchronizedCollection<object>();

            if (!_dictionary.TryAdd(eventType, collection))
            {
                _dictionary.TryGetValue(eventType, out collection);
            }

            return collection;
        }

        private class QueryHandlerAdapterAsync<TQuery, TResult> : IQueryHandlerAsync<IQuery<TResult>, TResult> where TQuery : IQuery<TResult>
        {
            private readonly IQueryHandlerAsync<TQuery, TResult> _handler;

            public QueryHandlerAdapterAsync(IQueryHandlerAsync<TQuery, TResult> handler)
            {
                _handler = handler;
            }

            public Task<TResult> HandleAsync(IQuery<TResult> query, CancellationToken token)
            {
                return _handler.HandleAsync((TQuery)query, token);
            }
        }

        private class CommandHandlerAdapterAsync<TCommand> : ICommandHandlerAsync<ICommand> where TCommand : ICommand
        {
            private readonly ICommandHandlerAsync<TCommand> _handler;

            public CommandHandlerAdapterAsync(ICommandHandlerAsync<TCommand> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(ICommand command, CancellationToken token)
            {
                return _handler.HandleAsync((TCommand)command, token);
            }
        }

        private class EventHandlerAdapterAsync<TEvent> : IEventHandlerAsync<IEvent> where TEvent : IEvent
        {
            private readonly IEventHandlerAsync<TEvent> _handler;

            public EventHandlerAdapterAsync(IEventHandlerAsync<TEvent> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(IEvent @event, CancellationToken token)
            {
                return _handler.HandleAsync((TEvent)@event, token);
            }
        }
    }
}
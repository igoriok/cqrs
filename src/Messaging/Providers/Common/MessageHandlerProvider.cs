using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public class MessageHandlerProvider : IMessageHandlerProvider, IDisposable
    {
        private readonly ConcurrentDictionary<Type, ICollection<object>> _dictionary;

        public MessageHandlerProvider()
        {
            _dictionary = new ConcurrentDictionary<Type, ICollection<object>>();
        }

        public void AddQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var collection = GetHandlersCollection(typeof(TQuery));

            collection.Add(handler);
        }

        public void RemoveQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var collection = GetHandlersCollection(typeof(TQuery));

            collection.Remove(handler);
        }

        public virtual IQueryHandler<IQuery<TResult>, TResult> GetQueryHandler<TResult>(Type queryType)
        {
            return GetHandlersCollection(queryType).Select(handler => WrapQueryHandler<TResult>(queryType, handler)).Single();
        }

        public void AddCommandHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            var collection = GetHandlersCollection(typeof(TCommand));

            collection.Add(handler);
        }

        public void RemoveCommandHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            var collection = GetHandlersCollection(typeof(TCommand));

            collection.Remove(handler);
        }

        public virtual ICommandHandler<ICommand> GetCommandHandler(Type commandType)
        {
            return GetHandlersCollection(commandType).Select(handler => WrapCommandHandler(commandType, handler)).Single();
        }

        public void AddEventHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var collection = GetHandlersCollection(typeof(TEvent));

            collection.Add(handler);
        }

        public void RemoveEventHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var collection = GetHandlersCollection(typeof(TEvent));

            collection.Remove(handler);
        }

        public virtual IEnumerable<IEventHandler<IEvent>> GetEventHandlers(Type eventType)
        {
            return GetHandlersCollection(eventType).Select(handler => WrapEventHandler(eventType, handler));
        }

        public void Dispose()
        {
            _dictionary.Clear();
        }

        protected IQueryHandler<IQuery<TResult>, TResult> WrapQueryHandler<TResult>(Type queryType, object handler)
        {
            var adapterType = typeof(QueryHandlerAdapter<,>).MakeGenericType(queryType, typeof(TResult));

            return (IQueryHandler<IQuery<TResult>, TResult>)Activator.CreateInstance(adapterType, handler);
        }

        protected ICommandHandler<ICommand> WrapCommandHandler(Type eventType, object handler)
        {
            var adapterType = typeof(CommandHandlerAdapter<>).MakeGenericType(eventType);

            return (ICommandHandler<ICommand>)Activator.CreateInstance(adapterType, handler);
        }

        protected IEventHandler<IEvent> WrapEventHandler(Type eventType, object handler)
        {
            var adapterType = typeof(EventHandlerAdapter<>).MakeGenericType(eventType);

            return (IEventHandler<IEvent>)Activator.CreateInstance(adapterType, handler);
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

        private class QueryHandlerAdapter<TQuery, TResult> : IQueryHandler<IQuery<TResult>, TResult> where TQuery : IQuery<TResult>
        {
            private readonly IQueryHandler<TQuery, TResult> _handler;

            public QueryHandlerAdapter(IQueryHandler<TQuery, TResult> handler)
            {
                _handler = handler;
            }

            public TResult Handle(IQuery<TResult> query)
            {
                return _handler.Handle((TQuery)query);
            }
        }

        private class CommandHandlerAdapter<TCommand> : ICommandHandler<ICommand> where TCommand : ICommand
        {
            private readonly ICommandHandler<TCommand> _handler;

            public CommandHandlerAdapter(ICommandHandler<TCommand> handler)
            {
                _handler = handler;
            }

            public void Handle(ICommand command)
            {
                _handler.Handle((TCommand)command);
            }
        }

        private class EventHandlerAdapter<TEvent> : IEventHandler<IEvent> where TEvent : IEvent
        {
            private readonly IEventHandler<TEvent> _handler;

            public EventHandlerAdapter(IEventHandler<TEvent> handler)
            {
                _handler = handler;
            }

            public void Handle(IEvent @event)
            {
                _handler.Handle((TEvent)@event);
            }
        }
    }
}
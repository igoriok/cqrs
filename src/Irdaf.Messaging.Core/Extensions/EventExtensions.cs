using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public static class EventExtensions
    {
        public static IEventHandler<TEvent> CreateEventHandler<TEvent>(this Action<TEvent, IMessageContext> handler) where TEvent : IEvent
        {
            return new ActionEventHandler<TEvent>(handler);
        }

        public static IEventHandlerAsync<TEvent> CreateEventHandlerAsync<TEvent>(this Func<TEvent, IMessageContext, CancellationToken, Task> handler) where TEvent : IEvent
        {
            return new ActionEventHandlerAsync<TEvent>(handler);
        }

        public static IEventHandlerAsync<TEvent> CreateEventHandlerAsync<TEvent>(this Action<TEvent, IMessageContext> handler) where TEvent : IEvent
        {
            return CreateEventHandlerAsync<TEvent>((e, c, t) => Task.Run(() => handler(e, c), t));
        }

        private class ActionEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
        {
            private readonly Action<TEvent, IMessageContext> _handler;

            public ActionEventHandler(Action<TEvent, IMessageContext> handler)
            {
                _handler = handler;
            }

            public void Handle(TEvent @event, IMessageContext context)
            {
                _handler(@event, context);
            }
        }

        private class ActionEventHandlerAsync<TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEvent
        {
            private readonly Func<TEvent, IMessageContext, CancellationToken, Task> _handler;

            public ActionEventHandlerAsync(Func<TEvent, IMessageContext, CancellationToken, Task> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(TEvent @event, IMessageContext context, CancellationToken cancellationToken)
            {
                return _handler(@event, context, cancellationToken);
            }
        }
    }
}
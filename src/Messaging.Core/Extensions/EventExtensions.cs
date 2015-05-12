using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class EventExtensions
    {
        public static IEventHandler<TEvent> CreateEventHandler<TEvent>(this Action<TEvent> handler) where TEvent : IEvent
        {
            return new ActionEventHandler<TEvent>(handler);
        }

        public static IEventHandlerAsync<TEvent> CreateEventHandlerAsync<TEvent>(this Func<TEvent, CancellationToken, Task> handler) where TEvent : IEvent
        {
            return new ActionEventHandlerAsync<TEvent>(handler);
        }

        public static IEventHandlerAsync<TEvent> CreateEventHandlerAsync<TEvent>(this Action<TEvent> handler) where TEvent : IEvent
        {
            return CreateEventHandlerAsync<TEvent>((e, t) => Task.Run(() => handler(e), t));
        }

        public static Task PublishAsync(this IEventServiceAsync service, IEvent @event)
        {
            return service.PublishAsync(@event, CancellationToken.None);
        }

        private class ActionEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
        {
            private readonly Action<TEvent> _handler;

            public ActionEventHandler(Action<TEvent> handler)
            {
                _handler = handler;
            }

            public void Handle(TEvent @event)
            {
                _handler(@event);
            }
        }

        private class ActionEventHandlerAsync<TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEvent
        {
            private readonly Func<TEvent, CancellationToken, Task> _handler;

            public ActionEventHandlerAsync(Func<TEvent, CancellationToken, Task> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(TEvent @event, CancellationToken token)
            {
                return _handler(@event, token);
            }
        }
    }
}
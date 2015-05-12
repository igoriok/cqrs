using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class SubscriptionExtensions
    {
        public static IDisposable Subscribe<TEvent>(this ISubscriptionService service, Action<TEvent> handler) where TEvent : IEvent
        {
            return service.Subscribe(handler.CreateEventHandler());
        }

        public static Task<IDisposable> SubscribeAsync<TEvent>(this ISubscriptionServiceAsync service, Action<TEvent> handler) where TEvent : IEvent
        {
            return service.SubscribeAsync(handler.CreateEventHandlerAsync(), CancellationToken.None);
        }

        public static Task<IDisposable> SubscribeAsync<TEvent>(this ISubscriptionServiceAsync service, Func<TEvent, CancellationToken, Task> handler) where TEvent : IEvent
        {
            return service.SubscribeAsync(handler.CreateEventHandlerAsync(), CancellationToken.None);
        }

        public static IEventHandler<TEvent> ToEventHandler<TEvent>(this IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return new AggregateEventHandler<TEvent>(handlers.ToArray());
        }

        public static IEventHandlerAsync<TEvent> ToEventHandlerAsync<TEvent>(this IEnumerable<IEventHandlerAsync<TEvent>> handlers) where TEvent : IEvent
        {
            return new AggregateEventHandlerAsync<TEvent>(handlers.ToArray());
        }

        private class AggregateEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
        {
            private readonly IEventHandler<TEvent>[] _handlers;

            public AggregateEventHandler(params IEventHandler<TEvent>[] handlers)
            {
                _handlers = handlers;
            }

            public void Handle(TEvent @event)
            {
                foreach (var handler in _handlers)
                {
                    handler.Handle(@event);
                }
            }
        }

        private class AggregateEventHandlerAsync<TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEvent
        {
            private readonly IEnumerable<IEventHandlerAsync<TEvent>> _list;

            public AggregateEventHandlerAsync(IEnumerable<IEventHandlerAsync<TEvent>> list)
            {
                _list = list;
            }

            public async Task HandleAsync(TEvent @event, CancellationToken token)
            {
                foreach (var handler in _list)
                {
                    await handler.HandleAsync(@event, token);
                }
            }
        }
    }
}
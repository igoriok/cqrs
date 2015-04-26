using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging
{
    public static class SubscriptionExtensions
    {
        public static IDisposable Subscribe<TEvent>(this IEventHandlerProvider provider, IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return new Subscription<TEvent>(handler, provider);
        }

        public static Task<IDisposable> SubscribeAsync<TEvent>(this IEventHandlerProviderAsync provider, IEventHandlerAsync<TEvent> handler) where TEvent : IEvent
        {
            return Task.FromResult<IDisposable>(new SubscriptionAsync<TEvent>(handler, provider));
        }

        private class Subscription<TEvent> : IEventHandler<IEvent>, IDisposable where TEvent : IEvent
        {
            private readonly IEventHandler<TEvent> _handler;
            private readonly IEventHandlerProvider _provider;

            public Subscription(IEventHandler<TEvent> handler, IEventHandlerProvider provider)
            {
                _provider = provider;
                _handler = handler;

                _provider.AddEventHandler(this);
            }

            public void Handle(IEvent @event)
            {
                _handler.Handle((TEvent)@event);
            }

            public void Dispose()
            {
                _provider.RemoveEventHandler(this);
            }
        }

        private class SubscriptionAsync<TEvent> : IEventHandlerAsync<IEvent>, IDisposable where TEvent : IEvent
        {
            private readonly IEventHandlerAsync<TEvent> _handler;
            private readonly IEventHandlerProviderAsync _provider;

            public SubscriptionAsync(IEventHandlerAsync<TEvent> handler, IEventHandlerProviderAsync provider)
            {
                _provider = provider;
                _handler = handler;

                _provider.AddEventHandlerAsync(this);
            }

            public Task HandleAsync(IEvent @event, CancellationToken token)
            {
                return _handler.HandleAsync((TEvent)@event, token);
            }

            public void Dispose()
            {
                _provider.RemoveEventHandlerAsync(this);
            }
        }
    }
}
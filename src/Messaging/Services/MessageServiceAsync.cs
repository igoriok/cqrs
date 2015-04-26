using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Services
{
    public class MessageServiceAsync : IMessageServiceAsync
    {
        private readonly IMessageHandlerProviderAsync _provider;

        public MessageServiceAsync(IMessageHandlerProviderAsync provider)
        {
            _provider = provider;
        }

        public virtual async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            var handler = await _provider.GetQueryHandlerAsync<TResult>(query.GetType(), token);

            return await handler.HandleAsync(query, token);
        }

        public virtual async Task ExecuteAsync(ICommand command, CancellationToken token)
        {
            var handler = await _provider.GetCommandHandlerAsync(command.GetType(), token);

            await handler.HandleAsync(command, token);
        }

        public virtual async Task PublishAsync(IEvent @event, CancellationToken token)
        {
            var handlers = await _provider.GetEventHandlersAsync(@event.GetType(), token);

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, token);
            }
        }

        public virtual Task<IDisposable> SubscribeAsync<TEvent>(IEventHandlerAsync<TEvent> handler, CancellationToken token) where TEvent : IEvent
        {
            return _provider.SubscribeAsync(handler);
        }
    }
}
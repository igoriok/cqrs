using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging.Decorators
{
    public class MessageServiceDecoratorAsync : IMessageServiceAsync
    {
        private readonly IMessageServiceAsync _messageService;
        private readonly IUnitOfWorkFactoryAsync _uowFactory;

        public MessageServiceDecoratorAsync(IMessageServiceAsync messageService, IUnitOfWorkFactoryAsync uowFactory)
        {
            _messageService = messageService;
            _uowFactory = uowFactory;
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            return _messageService.QueryAsync(query, token);
        }

        public async Task ExecuteAsync(ICommand command, CancellationToken token)
        {
            using (var uow = await _uowFactory.CreateAsync(token))
            {
                try
                {
                    await _messageService.ExecuteAsync(command, token);

                    await uow.CommitAsync();
                }
                catch
                {
                    uow.RollbackAsync(token).Wait(token);

                    throw;
                }
            }
        }

        public Task PublishAsync(IEvent @event, CancellationToken token)
        {
            return _messageService.PublishAsync(@event, token);
        }

        public Task<IDisposable> SubscribeAsync<TEvent>(IEventHandlerAsync<TEvent> handler, CancellationToken token) where TEvent : IEvent
        {
            return _messageService.SubscribeAsync(handler, token);
        }
    }
}
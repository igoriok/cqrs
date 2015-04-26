using System;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging.Decorators
{
    public class MessageServiceDecorator : IMessageService
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWorkFactory _uowFactory;

        public MessageServiceDecorator(IMessageService messageService, IUnitOfWorkFactory uowFactory)
        {
            _messageService = messageService;
            _uowFactory = uowFactory;
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            return _messageService.Query(query);
        }

        public void Execute(ICommand command)
        {
            using (var uow = _uowFactory.Create())
            {
                try
                {
                    _messageService.Execute(command);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();

                    throw;
                }
            }
        }

        public void Publish(IEvent @event)
        {
            _messageService.Publish(@event);
        }

        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return _messageService.Subscribe(handler);
        }
    }
}
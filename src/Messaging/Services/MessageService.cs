using System;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageHandlerProvider _provider;

        public MessageService(IMessageHandlerProvider provider)
        {
            _provider = provider;
        }

        public virtual TResult Query<TResult>(IQuery<TResult> query)
        {
            var handler = _provider.GetQueryHandler<TResult>(query.GetType());

            return handler.Handle(query);
        }

        public virtual void Execute(ICommand command)
        {
            var handler = _provider.GetCommandHandler(command.GetType());

            handler.Handle(command);
        }

        public virtual void Publish(IEvent @event)
        {
            var handlers = _provider.GetEventHandlers(@event.GetType());

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }

        public virtual IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return _provider.Subscribe(handler);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Adapters;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public class EventContext : MessageContext, IExecutor
    {
        public IEvent Event
        {
            get { return (IEvent)Message; }
        }

        public EventContext(IEvent @event)
            : base(@event)
        {
        }

        public void Execute(IHandlerRegistry registry)
        {
            var messageType = MessageType;
            var handlerType = typeof(IEventHandler<>).MakeGenericType(messageType);
            var adapterType = typeof(EventHandlerAdapter<>).MakeGenericType(messageType);

            var handler = (IEventHandler<IEvent>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this));

            handler.Handle(Event, this);
        }

        public Task ExecuteAsync(IHandlerRegistry registry, CancellationToken cancellationToken)
        {
            var messageType = MessageType;
            var handlerType = typeof(IEventHandlerAsync<>).MakeGenericType(messageType);
            var adapterType = typeof(EventHandlerAdapterAsync<>).MakeGenericType(messageType);

            var handler = (IEventHandlerAsync<IEvent>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this));

            return handler.HandleAsync(Event, this, cancellationToken);
        }
    }
}
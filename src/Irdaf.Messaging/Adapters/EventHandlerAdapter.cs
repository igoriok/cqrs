using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class EventHandlerAdapter<TEvent> : IEventHandler<IEvent> where TEvent : IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

        public EventHandlerAdapter(IEnumerable handlers)
        {
            _handlers = handlers.OfType<IEventHandler<TEvent>>().ToList();
        }

        public void Handle(IEvent @event, IMessageContext context)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle((TEvent)@event, context);
            }
        }
    }
}
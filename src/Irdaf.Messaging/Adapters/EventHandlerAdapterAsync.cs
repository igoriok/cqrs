using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class EventHandlerAdapterAsync<TEvent> : IEventHandlerAsync<IEvent> where TEvent : IEvent
    {
        private readonly IEnumerable<IEventHandlerAsync<TEvent>> _handlers;

        public EventHandlerAdapterAsync(IEnumerable handlers)
        {
            _handlers = handlers.OfType<IEventHandlerAsync<TEvent>>().ToList();
        }

        public async Task HandleAsync(IEvent @event, IMessageContext context, CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers)
            {
                await handler.HandleAsync((TEvent)@event, context, cancellationToken);
            }
        }
    }
}
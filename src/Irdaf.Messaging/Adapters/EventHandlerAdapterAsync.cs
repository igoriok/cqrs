using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class EventHandlerAdapterAsync<TEvent> : IEventHandlerAsync<IEvent> where TEvent : IEvent
    {
        private readonly IEnumerable<IEventHandlerAsync<TEvent>> _handler;

        public EventHandlerAdapterAsync(IEnumerable<IEventHandlerAsync<TEvent>> handler)
        {
            _handler = handler;
        }

        public async Task HandleAsync(IEvent @event, IMessageContext context, CancellationToken cancellationToken)
        {
            foreach (var handler in _handler)
            {
                await handler.HandleAsync((TEvent)@event, context, cancellationToken);
            }
        }
    }
}
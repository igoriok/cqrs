using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Pipeline.Convensions
{
    public class EventConvention<TEvent> : BaseMessageConvention<EventContext>
        where TEvent : IEvent
    {
        public EventConvention()
            : base(typeof(TEvent))
        {
        }

        public override Type GetHandlerBaseType()
        {
            return typeof(IEventHandler<>);
        }

        public override Type GetHandlerAsyncBaseType()
        {
            return typeof(IEventHandlerAsync<>);
        }

        public override Type GetHandlerType()
        {
            return GetHandlerBaseType().MakeGenericType(MessageType);
        }

        public override Type GetHandlerAsyncType()
        {
            return GetHandlerAsyncBaseType().MakeGenericType(MessageType);
        }

        protected override void Invoke(IList<object> handlers, EventContext context)
        {
            foreach (var handler in handlers.OfType<IEventHandler<TEvent>>())
            {
                handler.Handle((TEvent)context.Event, context);
            }

            foreach (var handler in handlers.OfType<IEventHandlerAsync<TEvent>>())
            {
                handler.HandleAsync((TEvent)context.Event, context, CancellationToken.None).GetAwaiter().GetResult();
            }
        }

        protected override async Task InvokeAsync(IList<object> handlers, EventContext context, CancellationToken cancellationToken)
        {
            foreach (var handler in handlers.OfType<IEventHandlerAsync<TEvent>>())
            {
                await handler.HandleAsync((TEvent)context.Event, context, cancellationToken);
            }

            foreach (var handler in handlers.OfType<IEventHandler<TEvent>>())
            {
                await Task.Run(() => handler.Handle((TEvent)context.Event, context), cancellationToken);
            }
        }
    }
}
using System;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline
{
    public sealed class EventContext : PipelineContext
    {
        public IEvent Event
        {
            get { return (IEvent)Message; }
        }

        public EventContext(IEvent @event)
            : base(@event, (IMessageConvention)Activator.CreateInstance(typeof(EventConvention<>).MakeGenericType(@event.GetType())))
        {
        }
    }
}
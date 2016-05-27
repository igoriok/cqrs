using Irdaf.Messaging.Dispatch.Executors;

namespace Irdaf.Messaging.Dispatch
{
    public class EventContext : MessageContext
    {
        public IEvent Event
        {
            get { return (IEvent)Message; }
        }

        public EventContext(IEvent @event)
            : base(@event)
        {
            this.Set<IExecutor>(new EventExecutor(this));
        }
    }
}
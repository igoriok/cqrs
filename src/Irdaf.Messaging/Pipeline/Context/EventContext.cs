namespace Irdaf.Messaging.Pipeline
{
    public sealed class EventContext : PipelineContext
    {
        public IEvent Event
        {
            get { return (IEvent)Message; }
        }

        public EventContext(IEvent @event, IPipelineContext parent = null)
            : base(@event, parent)
        {
        }
    }
}
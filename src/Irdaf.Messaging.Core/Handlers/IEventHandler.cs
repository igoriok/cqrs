namespace Irdaf.Messaging.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event, IMessageContext context);
    }
}
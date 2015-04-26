namespace Irdaf.Messaging.Services
{
    public interface IEventService
    {
        void Publish(IEvent @event);
    }
}
namespace Irdaf.Messaging.Services
{
    public interface IMessageService : IQueryService, ICommandService, IEventService, ISubscriptionService
    {
    }
}
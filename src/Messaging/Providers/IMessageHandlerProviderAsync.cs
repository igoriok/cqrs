namespace Irdaf.Messaging.Providers
{
    public interface IMessageHandlerProviderAsync : IQueryHandlerProviderAsync, ICommandHandlerProviderAsync, IEventHandlerProviderAsync
    {
    }
}
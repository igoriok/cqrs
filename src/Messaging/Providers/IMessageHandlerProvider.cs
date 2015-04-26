namespace Irdaf.Messaging.Providers
{
    public interface IMessageHandlerProvider : IQueryHandlerProvider, ICommandHandlerProvider, IEventHandlerProvider
    {
    }
}
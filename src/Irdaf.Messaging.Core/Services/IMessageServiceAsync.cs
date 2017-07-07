namespace Irdaf.Messaging.Services
{
    public interface IMessageServiceAsync : IQueryServiceAsync, ICommandServiceAsync, IEventServiceAsync
    {
    }
}
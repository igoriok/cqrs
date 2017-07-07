using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Services
{
    public static class ServiceExtensions
    {
        public static Task<TResult> QueryAsync<TResult>(this IQueryServiceAsync service, IQuery<TResult> query)
        {
            return service.QueryAsync(query, CancellationToken.None);
        }

        public static Task ExecuteAsync(this ICommandServiceAsync service, ICommand command)
        {
            return service.ExecuteAsync(command, CancellationToken.None);
        }

        public static Task PublishAsync(this IEventServiceAsync service, IEvent @event)
        {
            return service.PublishAsync(@event, CancellationToken.None);
        }
    }
}
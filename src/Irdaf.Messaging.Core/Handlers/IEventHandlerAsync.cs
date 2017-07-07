using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Handlers
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, IMessageContext context,  CancellationToken cancellationToken);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Services
{
    public interface IEventServiceAsync
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
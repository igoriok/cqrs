using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch
{
    public interface IDispatcher
    {
        void Dispatch(IMessageContext context);

        Task DispatchAsync(IMessageContext context, CancellationToken cancellationToken);
    }
}
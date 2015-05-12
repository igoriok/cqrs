using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Behaviors
{
    public interface IServiceBehaviorAsync
    {
        Task BeforeDispatch(IMessage message, CancellationToken token);

        Task AfterDispatch(IMessage message, CancellationToken token);
    }
}
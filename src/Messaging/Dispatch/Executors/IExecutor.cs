using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Executors
{
    public interface IExecutor
    {
        void Execute(IEnumerable handlers);

        Task ExecuteAsync(IEnumerable handlers, CancellationToken token);
    }
}
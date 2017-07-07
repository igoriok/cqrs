using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public interface IExecutor : IPipelineContext
    {
        void Execute(IHandlerRegistry registry);

        Task ExecuteAsync(IHandlerRegistry registry, CancellationToken cancellationToken);
    }
}
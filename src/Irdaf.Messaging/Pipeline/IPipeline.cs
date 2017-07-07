using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline
{
    public interface IPipeline
    {
        void Execute(IPipelineContext context);

        Task ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken);
    }
}
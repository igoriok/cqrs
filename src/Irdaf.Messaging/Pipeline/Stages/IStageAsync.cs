using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public interface IStageAsync
    {
        Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken);
    }
}
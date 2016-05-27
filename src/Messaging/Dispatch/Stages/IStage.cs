using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Stages
{
    public interface IStage
    {
        void Execute(IMessageContext context, Action next);

        Task ExecuteAsync(IMessageContext context, Func<Task> next, CancellationToken cancellationToken);
    }
}
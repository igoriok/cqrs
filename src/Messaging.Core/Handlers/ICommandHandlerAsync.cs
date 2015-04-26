using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Handlers
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Services
{
    public interface ICommandServiceAsync
    {
        Task ExecuteAsync(ICommand command, CancellationToken cancellationToken);
    }
}
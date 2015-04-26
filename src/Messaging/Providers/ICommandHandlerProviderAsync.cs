using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface ICommandHandlerProviderAsync
    {
        Task<ICommandHandlerAsync<ICommand>> GetCommandHandlerAsync(Type commandType, CancellationToken token);
    }
}
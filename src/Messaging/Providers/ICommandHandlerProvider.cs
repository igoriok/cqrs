using System;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface ICommandHandlerProvider
    {
        ICommandHandler<ICommand> GetCommandHandler(Type commandType);
    }
}
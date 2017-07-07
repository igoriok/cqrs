using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class CommandHandlerAdapterAsync<TCommand> : ICommandHandlerAsync<ICommand> where TCommand : ICommand
    {
        private readonly ICommandHandlerAsync<TCommand> _handler;

        public CommandHandlerAdapterAsync(ICommandHandlerAsync<TCommand> handler)
        {
            _handler = handler;
        }

        public Task HandleAsync(ICommand command, IMessageContext context, CancellationToken cancellationToken)
        {
            return _handler.HandleAsync((TCommand)command, context, cancellationToken);
        }
    }
}
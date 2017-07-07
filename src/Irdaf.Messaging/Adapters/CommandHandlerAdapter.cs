using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class CommandHandlerAdapter<TCommand> : ICommandHandler<ICommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public CommandHandlerAdapter(ICommandHandler<TCommand> handler)
        {
            _handler = handler;
        }

        public void Handle(ICommand command, IMessageContext context)
        {
            _handler.Handle((TCommand)command, context);
        }
    }
}
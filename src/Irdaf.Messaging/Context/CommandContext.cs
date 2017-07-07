using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Adapters;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public class CommandContext : MessageContext, IExecutor
    {
        public ICommand Command
        {
            get { return (ICommand)Message; }
        }

        public CommandContext(ICommand command)
            : base(command)
        {
        }

        public void Execute(IHandlerRegistry registry)
        {
            var messageType = MessageType;
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(messageType);
            var adapterType = typeof(CommandHandlerAdapter<>).MakeGenericType(messageType);

            var handler = (ICommandHandler<ICommand>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this).OfType<object>().Single());

            handler.Handle(Command, this);
        }

        public Task ExecuteAsync(IHandlerRegistry registry, CancellationToken cancellationToken)
        {
            var messageType = MessageType;
            var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(messageType);
            var adapterType = typeof(CommandHandlerAdapterAsync<>).MakeGenericType(messageType);

            var handler = (ICommandHandlerAsync<ICommand>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this).OfType<object>().Single());

            return handler.HandleAsync(Command, this, cancellationToken);
        }
    }
}
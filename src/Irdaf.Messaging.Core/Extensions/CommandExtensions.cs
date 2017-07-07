using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public static class CommandExtensions
    {
        public static ICommandHandler<TCommand> CreateCommandHandler<TCommand>(this Action<TCommand, IMessageContext> handler)
            where TCommand : ICommand
        {
            return new ActionCommandHandler<TCommand>(handler);
        }

        public static ICommandHandlerAsync<TCommand> CreateCommandHandlerAsync<TCommand>(this Func<TCommand, IMessageContext, CancellationToken, Task> handler)
            where TCommand : ICommand
        {
            return new ActionCommandHandlerAsync<TCommand>(handler);
        }

        private class ActionCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
        {
            private readonly Action<TCommand, IMessageContext> _handler;

            public ActionCommandHandler(Action<TCommand, IMessageContext> handler)
            {
                _handler = handler;
            }

            public void Handle(TCommand @event, IMessageContext context)
            {
                _handler(@event, context);
            }
        }

        private class ActionCommandHandlerAsync<TCommand> : ICommandHandlerAsync<TCommand> where TCommand : ICommand
        {
            private readonly Func<TCommand, IMessageContext, CancellationToken, Task> _handler;

            public ActionCommandHandlerAsync(Func<TCommand, IMessageContext, CancellationToken, Task> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(TCommand @event, IMessageContext context, CancellationToken cancellationToken)
            {
                return _handler(@event, context, cancellationToken);
            }
        }
    }
}
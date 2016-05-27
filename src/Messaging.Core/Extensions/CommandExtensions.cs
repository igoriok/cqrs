using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class CommandExtensions
    {
        public static ICommandHandler<TCommand> CreateCommandHandler<TCommand>(this Action<TCommand> handler)
            where TCommand : ICommand
        {
            return new ActionCommandHandler<TCommand>(handler);
        }

        public static ICommandHandlerAsync<TCommand> CreateCommandHandlerAsync<TCommand>(this Func<TCommand, CancellationToken, Task> handler)
            where TCommand : ICommand
        {
            return new ActionCommandHandlerAsync<TCommand>(handler);
        }

        public static Task ExecuteAsync(this ICommandServiceAsync service, ICommand command)
        {
            return service.ExecuteAsync(command, CancellationToken.None);
        }

        private class ActionCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
        {
            private readonly Action<TCommand> _handler;

            public ActionCommandHandler(Action<TCommand> handler)
            {
                _handler = handler;
            }

            public void Handle(TCommand @event)
            {
                _handler(@event);
            }
        }

        private class ActionCommandHandlerAsync<TCommand> : ICommandHandlerAsync<TCommand> where TCommand : ICommand
        {
            private readonly Func<TCommand, CancellationToken, Task> _handler;

            public ActionCommandHandlerAsync(Func<TCommand, CancellationToken, Task> handler)
            {
                _handler = handler;
            }

            public Task HandleAsync(TCommand @event, CancellationToken token)
            {
                return _handler(@event, token);
            }
        }
    }
}
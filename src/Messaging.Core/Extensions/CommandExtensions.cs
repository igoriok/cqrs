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

        public static TEvent Execute<TEvent>(this IMessageService service, ICommand command) where TEvent : IEvent
        {
            var handler = new TaskEventHandler<TEvent>();

            using (service.Subscribe(handler))
            {
                service.Execute(command);

                return handler.Task.Result;
            }
        }
        
        public static Task ExecuteAsync(this ICommandServiceAsync service, ICommand command)
        {
            return service.ExecuteAsync(command, CancellationToken.None);
        }

        public static Task<TEvent> ExecuteAsync<TEvent>(this IMessageServiceAsync service, ICommand command) where TEvent : IEvent
        {
            return ExecuteAsync<TEvent>(service, command, CancellationToken.None);
        }

        public static async Task<TEvent> ExecuteAsync<TEvent>(this IMessageServiceAsync service, ICommand command, CancellationToken token) where TEvent : IEvent
        {
            var handler = new TaskEventHandlerAsync<TEvent>();

            using (await service.SubscribeAsync(handler, token))
            {
                await service.ExecuteAsync(command, token);

                return await handler.Task;
            }
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

        private class TaskEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
        {
            private readonly TaskCompletionSource<TEvent> _source;

            public Task<TEvent> Task
            {
                get { return _source.Task; }
            }

            public TaskEventHandler()
            {
                _source = new TaskCompletionSource<TEvent>();
            }

            public void Handle(TEvent @event)
            {
                _source.SetResult(@event);
            }
        }

        private class TaskEventHandlerAsync<TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEvent
        {
            private readonly TaskCompletionSource<TEvent> _source;

            public Task<TEvent> Task
            {
                get { return _source.Task; }
            }

            public TaskEventHandlerAsync()
            {
                _source = new TaskCompletionSource<TEvent>();
            }

            public Task HandleAsync(TEvent @event, CancellationToken token)
            {
                return System.Threading.Tasks.Task.Run(() => _source.SetResult(@event), token);
            }
        }
    }
}
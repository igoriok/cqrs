using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class CommandExtensions
    {
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
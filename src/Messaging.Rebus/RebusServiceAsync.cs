using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;
using Rebus;
using Rebus.Shared;

namespace Irdaf.Messaging.Rebus
{
    public class RebusServiceAsync : IMessageServiceAsync, IHandleMessagesAsync<object>
    {
        private readonly ConcurrentDictionary<object, TaskCompletionSource<object>> _queries
             = new ConcurrentDictionary<object, TaskCompletionSource<object>>();

        private readonly IBus _bus;
        private readonly IMessageServiceAsync _service;

        public RebusServiceAsync(IBus bus, IMessageServiceAsync service)
        {
            _bus = bus;
            _service = service;
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            return Task.Run(
                () =>
                {
                    var task = new TaskCompletionSource<object>();
                    var correlationId = Guid.NewGuid().ToString();

                    _queries.TryAdd(correlationId, task);

                    _bus.AttachHeader(query, Headers.CorrelationId, correlationId);
                    _bus.Send(query);

                    return (TResult)task.Task.Result;
                },
                token);
        }

        public Task ExecuteAsync(ICommand command, CancellationToken token)
        {
            return Task.Run(() => _bus.Send(command), token);
        }

        public Task PublishAsync(IEvent @event, CancellationToken token)
        {
            return Task.Run(() => _bus.Send(@event), token);
        }

        public Task<IDisposable> SubscribeAsync<TEvent>(IEventHandlerAsync<TEvent> handler, CancellationToken token) where TEvent : IEvent
        {
            return _service.SubscribeAsync(handler, token);
        }

        public Task Handle(object message)
        {
            return Task.Run(() =>
            {
                var context = MessageContext.GetCurrent();
                if (context == null)
                    return;

                var correlationId = context.Headers[Headers.CorrelationId];

                TaskCompletionSource<object> task;

                if (_queries.TryRemove(correlationId, out task))
                {
                    task.SetResult(message);
                }
            });
        }
    }
}
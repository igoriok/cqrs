using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Services;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Messages;

namespace Irdaf.Messaging.Rebus
{
    public class RebusService : IHandleMessages<object>, IMessageService, IMessageServiceAsync
    {
        private readonly ConcurrentDictionary<object, TaskCompletionSource<object>> _tasks =
            new ConcurrentDictionary<object, TaskCompletionSource<object>>();

        private readonly IBus _bus;

        public RebusService(IBus bus)
        {
            _bus = bus;
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            return SendAndWait<TResult>(query).Result;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            return await SendAndWait<TResult>(query);
        }

        public void Execute(ICommand command)
        {
            SendAndWait<object>(command).Wait();
        }

        public async Task ExecuteAsync(ICommand command, CancellationToken token)
        {
            await SendAndWait<object>(command);
        }

        public void Publish(IEvent @event)
        {
            _bus.Send(@event).Wait();
        }

        public async Task PublishAsync(IEvent @event, CancellationToken token)
        {
            await _bus.Send(@event);
        }

        async Task IHandleMessages<object>.Handle(object message)
        {
            var context = global::Rebus.Pipeline.MessageContext.Current;

            var correlationId = context.Message.Headers[Headers.CorrelationId];

            TaskCompletionSource<object> task;

            if (_tasks.TryRemove(correlationId, out task))
            {
                task.SetResult(message);
            }

            await Task.Yield();
        }

        private async Task<TResult> SendAndWait<TResult>(IMessage message)
        {
            var task = new TaskCompletionSource<object>();
            var correlationId = Guid.NewGuid().ToString();

            _tasks.TryAdd(correlationId, task);

            await _bus
                .Send(message, new Dictionary<string, string>
                {
                    { Headers.CorrelationId, correlationId }
                });

            return (TResult)task.Task.Result;
        }
    }
}
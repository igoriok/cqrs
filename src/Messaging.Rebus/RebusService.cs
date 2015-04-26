using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;
using Rebus;
using Rebus.Shared;

namespace Irdaf.Messaging.Rebus
{
    public class RebusService : IMessageService, IHandleMessages<object>
    {
        private readonly ConcurrentDictionary<object, TaskCompletionSource<object>> _queries
            = new ConcurrentDictionary<object, TaskCompletionSource<object>>();

        private readonly IBus _bus;
        private readonly IMessageService _service;

        public RebusService(IBus bus, IMessageService service)
        {
            _bus = bus;
            _service = service;
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            var task = new TaskCompletionSource<object>();
            var correlationId = Guid.NewGuid().ToString();

            _queries.TryAdd(correlationId, task);

            _bus.AttachHeader(query, Headers.CorrelationId, correlationId);
            _bus.Send(query);

            return (TResult)task.Task.Result;
        }

        public void Execute(ICommand command)
        {
            _bus.Send(command);
        }

        public void Publish(IEvent @event)
        {
            _bus.Send(@event);
        }

        public void Handle(object message)
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
        }

        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return _service.Subscribe(handler);
        }
    }
}
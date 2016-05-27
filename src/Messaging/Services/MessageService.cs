using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Dispatch;

namespace Irdaf.Messaging.Services
{
    public class MessageService : IMessageService, IMessageServiceAsync
    {
        private readonly IDispatcher _dispatcher;

        public MessageService(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public virtual TResult Query<TResult>(IQuery<TResult> query)
        {
            using (var context = new QueryContext<TResult>(query))
            {
                _dispatcher.Dispatch(context);

                return context.Result;
            }
        }

        public virtual async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            using (var context = new QueryContext<TResult>(query))
            {
                await _dispatcher.DispatchAsync(context, token);

                return context.Result;
            }
        }

        public virtual void Execute(ICommand command)
        {
            using (var context = new CommandContext(command))
            {
                _dispatcher.Dispatch(context);
            }
        }

        public virtual async Task ExecuteAsync(ICommand command, CancellationToken token)
        {
            using (var context = new CommandContext(command))
            {
                await _dispatcher.DispatchAsync(context, token);
            }
        }

        public virtual void Publish(IEvent @event)
        {
            using (var context = new EventContext(@event))
            {
                _dispatcher.Dispatch(context);
            }
        }

        public virtual async Task PublishAsync(IEvent @event, CancellationToken token)
        {
            using (var context = new EventContext(@event))
            {
                await _dispatcher.DispatchAsync(context, token);
            }
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Services
{
    public class MessageService : IMessageService, IMessageServiceAsync
    {
        private readonly IPipeline _pipeline;

        public MessageService(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public virtual TResult Query<TResult>(IQuery<TResult> query)
        {
            using (var context = new QueryContext<TResult>(query))
            {
                _pipeline.Execute(context);

                return context.Result;
            }
        }

        public virtual async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            using (var context = new QueryContext<TResult>(query))
            {
                await _pipeline.ExecuteAsync(context, cancellationToken);

                return context.Result;
            }
        }

        public virtual void Execute(ICommand command)
        {
            using (var context = new CommandContext(command))
            {
                _pipeline.Execute(context);
            }
        }

        public virtual async Task ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            using (var context = new CommandContext(command))
            {
                await _pipeline.ExecuteAsync(context, cancellationToken);
            }
        }

        public virtual void Publish(IEvent @event)
        {
            using (var context = new EventContext(@event))
            {
                _pipeline.Execute(context);
            }
        }

        public virtual async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            using (var context = new EventContext(@event))
            {
                await _pipeline.ExecuteAsync(context, cancellationToken);
            }
        }
    }
}
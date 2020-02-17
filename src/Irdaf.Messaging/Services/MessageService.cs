using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Pipeline;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Services
{
    public class MessageService : IMessageService, IMessageServiceAsync
    {
        private readonly IPipeline _pipeline;
        private readonly IPipelineContext _parentContext;

        private MessageService(IPipeline pipeline, IPipelineContext parentContext = null)
        {
            _pipeline = pipeline;
            _parentContext = parentContext;
        }

        public virtual TResult Query<TResult>(IQuery<TResult> query)
        {
            using (var context = new QueryContext<TResult>(query, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(QueryConvention<,>).MakeGenericType(query.GetType(), typeof(TResult)));

                AttachServices(context, convention);

                _pipeline.Execute(context);

                return context.Result;
            }
        }

        public virtual async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            using (var context = new QueryContext<TResult>(query, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(QueryConvention<,>).MakeGenericType(query.GetType(), typeof(TResult)));

                AttachServices(context, convention, cancellationToken);

                await _pipeline.ExecuteAsync(context, cancellationToken);

                return context.Result;
            }
        }

        public virtual void Execute(ICommand command)
        {
            using (var context = new CommandContext(command, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(CommandConvention<>).MakeGenericType(command.GetType()));

                AttachServices(context, convention);

                _pipeline.Execute(context);
            }
        }

        public virtual async Task ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            using (var context = new CommandContext(command, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(CommandConvention<>).MakeGenericType(command.GetType()));

                AttachServices(context, convention, cancellationToken);

                await _pipeline.ExecuteAsync(context, cancellationToken);
            }
        }

        public virtual void Publish(IEvent @event)
        {
            using (var context = new EventContext(@event, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(EventConvention<>).MakeGenericType(@event.GetType()));

                AttachServices(context, convention);

                _pipeline.Execute(context);
            }
        }

        public virtual async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            using (var context = new EventContext(@event, _parentContext))
            {
                var convention = (IMessageConvention)Activator.CreateInstance(typeof(EventConvention<>).MakeGenericType(@event.GetType()));

                AttachServices(context, convention, cancellationToken);

                await _pipeline.ExecuteAsync(context, cancellationToken);
            }
        }

        private void AttachServices(IPipelineContext context, IMessageConvention convention, CancellationToken cancellationToken = default(CancellationToken))
        {
            var child = new MessageService(_pipeline, context);

            context.Set(_pipeline);
            context.Set(convention);
            context.Set(cancellationToken);
            context.Set<IQueryService>(child);
            context.Set<IQueryServiceAsync>(child);
            context.Set<ICommandService>(child);
            context.Set<ICommandServiceAsync>(child);
            context.Set<IEventService>(child);
            context.Set<IEventServiceAsync>(child);
            context.Set<IMessageService>(child);
            context.Set<IMessageServiceAsync>(child);
        }
    }
}
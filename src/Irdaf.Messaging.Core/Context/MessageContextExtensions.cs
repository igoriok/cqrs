using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class MessageContextExtensions
    {
        public static T Get<T>(this IMessageContext context, string key)
        {
            if (context.Contains(key))
            {
                return (T)context.Get(key);
            }

            return default(T);
        }

        public static T Get<T>(this IMessageContext context)
        {
            return context.Get<T>(typeof(T).FullName);
        }

        public static T Query<T>(this IMessageContext context, IQuery<T> query)
        {
            return context.Get<IQueryService>().Query(query);
        }

        public static Task<T> QueryAsync<T>(this IMessageContext context, IQuery<T> query)
        {
            return context.Get<IQueryServiceAsync>().QueryAsync(query, context.Get<CancellationToken>());
        }

        public static Task<T> QueryAsync<T>(this IMessageContext context, IQuery<T> query, CancellationToken cancellationToken)
        {
            return context.Get<IQueryServiceAsync>().QueryAsync(query, cancellationToken);
        }

        public static void Execute(this IMessageContext context, ICommand command)
        {
            context.Get<ICommandService>().Execute(command);
        }

        public static Task ExecuteAsync(this IMessageContext context, ICommand command)
        {
            return context.Get<ICommandServiceAsync>().ExecuteAsync(command, context.Get<CancellationToken>());
        }

        public static Task ExecuteAsync(this IMessageContext context, ICommand command, CancellationToken cancellationToken)
        {
            return context.Get<ICommandServiceAsync>().ExecuteAsync(command, cancellationToken);
        }

        public static void Publish(this IMessageContext context, IEvent @event)
        {
            context.Get<IEventService>().Publish(@event);
        }

        public static Task PublishAsync(this IMessageContext context, IEvent @event)
        {
            return context.Get<IEventServiceAsync>().PublishAsync(@event, context.Get<CancellationToken>());
        }

        public static Task PublishAsync(this IMessageContext context, IEvent @event, CancellationToken cancellationToken)
        {
            return context.Get<IEventServiceAsync>().PublishAsync(@event, cancellationToken);
        }
    }
}
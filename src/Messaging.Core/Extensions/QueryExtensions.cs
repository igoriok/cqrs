using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class QueryExtensions
    {
        public static IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>(this Func<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            return new ActionQueryHandler<TQuery, TResult>(handler);
        }

        public static IQueryHandlerAsync<TQuery, TResult> CreateQueryHandlerAsync<TQuery, TResult>(this Func<TQuery, CancellationToken, Task<TResult>> handler) where TQuery : IQuery<TResult>
        {
            return new ActionQueryHandlerAsync<TQuery, TResult>(handler);
        }

        public static Task<TResult> QueryAsync<TResult>(this IQueryServiceAsync service, IQuery<TResult> query)
        {
            return service.QueryAsync(query, CancellationToken.None);
        }

        private class ActionQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
        {
            private readonly Func<TQuery, TResult> _handler;

            public ActionQueryHandler(Func<TQuery, TResult> handler)
            {
                _handler = handler;
            }

            public TResult Handle(TQuery @event)
            {
                return _handler(@event);
            }
        }

        private class ActionQueryHandlerAsync<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult> where TQuery : IQuery<TResult>
        {
            private readonly Func<TQuery, CancellationToken, Task<TResult>> _handler;

            public ActionQueryHandlerAsync(Func<TQuery, CancellationToken, Task<TResult>> handler)
            {
                _handler = handler;
            }

            public Task<TResult> HandleAsync(TQuery @event, CancellationToken token)
            {
                return _handler(@event, token);
            }
        }
    }
}
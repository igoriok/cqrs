using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public static class QueryExtensions
    {
        public static IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>(this Func<TQuery, IMessageContext, TResult> handler) where TQuery : IQuery<TResult>
        {
            return new ActionQueryHandler<TQuery, TResult>(handler);
        }

        public static IQueryHandlerAsync<TQuery, TResult> CreateQueryHandlerAsync<TQuery, TResult>(this Func<TQuery, IMessageContext, CancellationToken, Task<TResult>> handler) where TQuery : IQuery<TResult>
        {
            return new ActionQueryHandlerAsync<TQuery, TResult>(handler);
        }

        private class ActionQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
        {
            private readonly Func<TQuery, IMessageContext, TResult> _handler;

            public ActionQueryHandler(Func<TQuery, IMessageContext, TResult> handler)
            {
                _handler = handler;
            }

            public TResult Handle(TQuery @event, IMessageContext context)
            {
                return _handler(@event, context);
            }
        }

        private class ActionQueryHandlerAsync<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult> where TQuery : IQuery<TResult>
        {
            private readonly Func<TQuery, IMessageContext, CancellationToken, Task<TResult>> _handler;

            public ActionQueryHandlerAsync(Func<TQuery, IMessageContext, CancellationToken, Task<TResult>> handler)
            {
                _handler = handler;
            }

            public Task<TResult> HandleAsync(TQuery @event, IMessageContext context, CancellationToken cancellationToken)
            {
                return _handler(@event, context, cancellationToken);
            }
        }
    }
}
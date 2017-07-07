using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class QueryHandlerAdapterAsync<TQuery, TResult> : IQueryHandlerAsync<IQuery<TResult>, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandlerAsync<TQuery, TResult> _handler;

        public QueryHandlerAdapterAsync(IQueryHandlerAsync<TQuery, TResult> handler)
        {
            _handler = handler;
        }

        public Task<TResult> HandleAsync(IQuery<TResult> query, IMessageContext context, CancellationToken cancellationToken)
        {
            return _handler.HandleAsync((TQuery)query, context, cancellationToken);
        }
    }
}
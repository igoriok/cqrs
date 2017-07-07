using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Adapters
{
    public class QueryHandlerAdapter<TQuery, TResult> : IQueryHandler<IQuery<TResult>, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;

        public QueryHandlerAdapter(IQueryHandler<TQuery, TResult> handler)
        {
            _handler = handler;
        }

        public TResult Handle(IQuery<TResult> query, IMessageContext context)
        {
            return _handler.Handle((TQuery)query, context);
        }
    }
}
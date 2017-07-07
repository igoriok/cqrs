using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Adapters;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging
{
    public class QueryContext<TResult> : MessageContext, IExecutor
    {
        private TResult _result;
        private bool _hasResult;

        public IQuery<TResult> Query
        {
            get { return (IQuery<TResult>)Message; }
        }

        public TResult Result
        {
            get
            {
                if (!_hasResult)
                    throw new InvalidOperationException("Result not set");

                return _result;
            }
            set
            {
                _result = value;
                _hasResult = true;
            }
        }

        public QueryContext(IQuery<TResult> query)
            : base(query)
        {
        }

        public void Execute(IHandlerRegistry registry)
        {
            var messageType = MessageType;
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(messageType, typeof(TResult));
            var adapterType = typeof(QueryHandlerAdapter<,>).MakeGenericType(messageType, typeof(TResult));

            var handler = (IQueryHandler<IQuery<TResult>, TResult>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this).OfType<object>().Single());

            Result = handler.Handle(Query, this);
        }

        public async Task ExecuteAsync(IHandlerRegistry registry, CancellationToken cancellationToken)
        {
            var messageType = MessageType;
            var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(messageType, typeof(TResult));
            var adapterType = typeof(QueryHandlerAdapterAsync<,>).MakeGenericType(messageType, typeof(TResult));

            var handler = (IQueryHandlerAsync<IQuery<TResult>, TResult>)Activator.CreateInstance(adapterType, registry.GetHandlers(handlerType, this).OfType<object>().Single());

            Result = await handler.HandleAsync(Query, this, cancellationToken);
        }
    }
}
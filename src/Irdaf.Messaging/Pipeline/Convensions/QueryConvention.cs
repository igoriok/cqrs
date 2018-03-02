using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Pipeline.Convensions
{
    public class QueryConvention<TQuery, TResult> : BaseMessageConvention<QueryContext<TResult>>
        where TQuery : IQuery<TResult>
    {
        public Type ResultType { get; }

        public QueryConvention()
            : base(typeof(TQuery))
        {
            ResultType = typeof(TResult);
        }

        public override Type GetHandlerBaseType()
        {
            return typeof(IQueryHandler<,>);
        }

        public override Type GetHandlerAsyncBaseType()
        {
            return typeof(IQueryHandlerAsync<,>);
        }

        public override Type GetHandlerType()
        {
            return GetHandlerBaseType().MakeGenericType(MessageType, ResultType);
        }

        public override Type GetHandlerAsyncType()
        {
            return GetHandlerAsyncBaseType().MakeGenericType(MessageType, ResultType);
        }

        protected override void Invoke(IList<object> handlers, QueryContext<TResult> context)
        {
            var handler = handlers.Single();

            if (handler is IQueryHandler<TQuery, TResult> syncHandler)
            {
                syncHandler.Handle((TQuery)context.Query, context);
            }
            else if (handler is IQueryHandlerAsync<TQuery, TResult> asyncHandler)
            {
                asyncHandler.HandleAsync((TQuery)context.Query, context, CancellationToken.None).GetAwaiter().GetResult();
            }
            else
            {
                throw new InvalidOperationException("Handlers not found");
            }
        }

        protected override async Task InvokeAsync(IList<object> handlers, QueryContext<TResult> context, CancellationToken cancellationToken)
        {
            var handler = handlers.Single();

            if (handler is IQueryHandlerAsync<TQuery, TResult> asyncHandler)
            {
                await asyncHandler.HandleAsync((TQuery)context.Query, context, cancellationToken);
            }
            else if (handler is IQueryHandler<TQuery, TResult> syncHandler)
            {
                await Task.Run(() => syncHandler.Handle((TQuery)context.Query, context), cancellationToken);
            }
            else
            {
                throw new InvalidOperationException("Handlers not found");
            }
        }
    }
}
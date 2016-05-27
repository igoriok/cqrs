using System;
using Irdaf.Messaging.Dispatch.Executors;

namespace Irdaf.Messaging.Dispatch
{
    public class QueryContext<TResult> : MessageContext
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
            this.Set<IExecutor>(new QueryExecutor<TResult>(this));
        }
    }
}
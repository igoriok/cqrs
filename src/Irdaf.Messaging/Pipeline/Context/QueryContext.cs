using System;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline
{
    public sealed class QueryContext<TResult> : PipelineContext
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
            : base(query, (IMessageConvention)Activator.CreateInstance(typeof(QueryConvention<,>).MakeGenericType(query.GetType(), typeof(TResult))))
        {
        }
    }
}
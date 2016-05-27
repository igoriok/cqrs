using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Executors
{
    public class QueryExecutor<TResult> : IExecutor
    {
        private readonly QueryContext<TResult> _context;

        public QueryExecutor(QueryContext<TResult> context)
        {
            _context = context;
        }

        public void Execute(IEnumerable handlers)
        {
            var signleHandler = handlers.OfType<dynamic>().Single();

            _context.Result = signleHandler.Handle(_context.Query);
        }

        public async Task ExecuteAsync(IEnumerable handlers, CancellationToken token)
        {
            var signleHandler = handlers.OfType<dynamic>().Single();

            _context.Result = await signleHandler.HandleAsync(_context.Query, token);
        }
    }
}
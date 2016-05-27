using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Executors
{
    public class EventExecutor : IExecutor
    {
        private readonly EventContext _context;

        public EventExecutor(EventContext context)
        {
            _context = context;
        }

        public void Execute(IEnumerable handlers)
        {
            foreach (dynamic handler in handlers)
            {
                handler.Handle(_context.Message);
            }
        }

        public async Task ExecuteAsync(IEnumerable handlers, CancellationToken token)
        {
            foreach (dynamic handler in handlers)
            {
                await handler.HandleAsync(_context.Message, token);
            }
        }
    }
}
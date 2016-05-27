using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Executors
{
    public class CommandExecutor : IExecutor
    {
        private readonly CommandContext _context;

        public CommandExecutor(CommandContext context)
        {
            _context = context;
        }

        public void Execute(IEnumerable handlers)
        {
            var signleHandler = handlers.OfType<dynamic>().Single();

            signleHandler.Handle(_context.Command);
        }

        public async Task ExecuteAsync(IEnumerable handlers, CancellationToken token)
        {
            var signleHandler = handlers.OfType<dynamic>().Single();

            await signleHandler.HandleAsync(_context.Command, token);
        }
    }
}
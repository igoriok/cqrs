using System;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Bus;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class RebusStage : IStage
    {
        private readonly IBus _bus;
        private readonly Func<IPipelineContext, bool> _async;

        public RebusStage(IBus bus, Func<IPipelineContext, bool> async)
        {
            _bus = bus;
            _async = async;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            if (_async(context))
            {
                _bus.Advanced.SyncBus.Send(context.Message);
            }
            else
            {
                next();
            }
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            if (_async(context))
            {
                await _bus.Send(context.Message);
            }
            else
            {
                await next();
            }
        }
    }
}
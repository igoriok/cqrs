using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Mapping;
using Irdaf.Messaging.Rebus;
using Rebus.Bus;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class RebusStage : IStage
    {
        private readonly IBus _bus;
        private readonly Func<IPipelineContext, bool> _async;
        private readonly IContextMapper _mapper;

        public RebusStage(IBus bus, Func<IPipelineContext, bool> async, IContextMapper mapper)
        {
            _bus = bus;
            _async = async;
            _mapper = mapper;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            var defer = context.Message as DeferMessage;

            if (defer != null || _async(context) && RebusContext.Current == null)
            {
                var headers = _mapper.Map(context);

                if (defer != null)
                {
                    _bus.Advanced.SyncBus.Defer(defer.Delay, defer.Message, headers);
                }
                else if (context is EventContext)
                {
                    _bus.Advanced.SyncBus.Publish(context.Message, headers);
                }
                else
                {
                    _bus.Advanced.SyncBus.Send(context.Message, headers);
                }
            }
            else
            {
                var current = RebusContext.Current;

                using (RebusContext.Assing(null))
                {
                    if (current != null)
                    {
                        _mapper.Map(current.Headers, context);
                    }

                    next();
                }
            }
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var defer = context.Message as DeferMessage;

            if (defer != null || _async(context) && RebusContext.Current == null)
            {
                var headers = _mapper.Map(context);

                if (defer != null)
                {
                    await _bus.Defer(defer.Delay, defer.Message, headers).ConfigureAwait(false);
                }
                else if (context is EventContext)
                {
                    await _bus.Publish(context.Message, headers).ConfigureAwait(false);
                }
                else
                {
                    await _bus.Send(context.Message, headers).ConfigureAwait(false);
                }
            }
            else
            {
                var current = RebusContext.Current;

                using (RebusContext.Assing(null))
                {
                    if (current != null)
                    {
                        _mapper.Map(current.Headers, context);
                    }

                    await next();
                }
            }
        }
    }
}
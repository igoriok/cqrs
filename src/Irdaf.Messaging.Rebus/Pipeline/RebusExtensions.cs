using System;
using Irdaf.Messaging.Pipeline.Stages;
using Rebus.Bus;

namespace Irdaf.Messaging.Pipeline
{
    public static class RebusExtensions
    {
        public static PipelineBuilder UseRebus(this PipelineBuilder builder, IBus bus, Func<IPipelineContext, bool> async)
        {
            return builder
                .Use(ctx => new RebusStage(bus, async));
        }
    }
}
using System;
using System.Collections.Generic;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public class PipelineBuilder
    {
        private readonly List<IHandlerSource> _sources;
        private readonly List<Func<IPipelineContext, IStage>> _stages;

        public PipelineBuilder()
        {
            _sources = new List<IHandlerSource>();
            _stages = new List<Func<IPipelineContext, IStage>>();

            Use(ctx => new InvokeHandlerStage(new DefaultHandlerRegistry(_sources)));
        }

        public PipelineBuilder From(IHandlerSource source)
        {
            _sources.Add(source);

            return this;
        }

        public PipelineBuilder Use(Func<IPipelineContext, IStage> stage)
        {
            _stages.Insert(0, stage);

            return this;
        }

        public IPipeline Build()
        {
            return new DefaultPipeline(_stages);
        }
    }
}
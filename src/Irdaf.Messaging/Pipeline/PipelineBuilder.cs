using System;
using System.Collections.Generic;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public class PipelineBuilder
    {
        private readonly List<IHandlerSource> _sources;
        private readonly List<Func<IPipelineContext, object>> _stages;

        public PipelineBuilder()
        {
            _sources = new List<IHandlerSource>();
            _stages = new List<Func<IPipelineContext, object>>();

            Use(ctx => new InvokeHandlerStage());
            Use(ctx => new ResolveHandlerStage(new DefaultHandlerRegistry(_sources)));
        }

        public PipelineBuilder From(IHandlerSource source)
        {
            _sources.Add(source);

            return this;
        }

        public PipelineBuilder Use(object stage)
        {
            return Use(ctx => stage);
        }

        public PipelineBuilder Use<TStage>()
            where TStage : new()
        {
            return Use(ctx => new TStage());
        }

        public PipelineBuilder Use(Func<IPipelineContext, object> stage)
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
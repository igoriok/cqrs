using System;
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public static class PipelineExtensions
    {
        public static PipelineBuilder UseDefaults(this PipelineBuilder builder)
        {
            return builder;
        }

        public static PipelineBuilder UseUnitOfWork<TUnitOfWork>(this PipelineBuilder builder, Func<IPipelineContext, TUnitOfWork> factory, Action<TUnitOfWork> commit, Action<TUnitOfWork> rollback)
            where TUnitOfWork : class, IDisposable
        {
            return builder.Use(ctx => new UnitOfWorkStage<TUnitOfWork>(factory, commit, rollback));
        }
    }
}
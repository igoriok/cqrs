using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseContainerStage<TContainer> : IStage, IStageAsync
        where TContainer : class, IDisposable
    {
        protected abstract TContainer CreateChildContainer(IPipelineContext context);

        public void Execute(IPipelineContext context, Action next)
        {
            using (var container = CreateChildContainer(context))
            {
                context.Set(container);

                try
                {
                    next();
                }
                finally
                {
                    context.Remove<TContainer>();
                }
            }
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (var container = CreateChildContainer(context))
            {
                context.Set(container);

                try
                {
                    await next();
                }
                finally
                {
                    context.Remove<TContainer>();
                }
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseUnitOfWorkStage<TUnitOfWork> : IStage, IStageAsync
        where TUnitOfWork : class, IDisposable
    {
        protected abstract TUnitOfWork Create(IPipelineContext context);

        protected abstract void Commit(TUnitOfWork unitOfWork, IPipelineContext context);

        protected abstract bool Rollback(TUnitOfWork unitOfWork, IPipelineContext context, Exception exception);

        public virtual void Execute(IPipelineContext context, Action next)
        {
            using (var uow = Create(context))
            {
                context.Set(uow);

                try
                {
                    next();

                    Commit(uow, context);
                }
                catch (Exception exception)
                {
                    if (Rollback(uow, context, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    context.Remove<TUnitOfWork>();
                }
            }
        }

        public virtual async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (var uow = Create(context))
            {
                context.Set(uow);

                try
                {
                    await next();

                    Commit(uow, context);
                }
                catch (Exception exception)
                {
                    if (Rollback(uow, context, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    context.Remove<TUnitOfWork>();
                }
            }
        }
    }
}
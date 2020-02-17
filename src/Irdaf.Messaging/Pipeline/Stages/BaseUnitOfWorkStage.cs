using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseUnitOfWorkStage<TUnitOfWork> : IStage, IStageAsync
        where TUnitOfWork : class, IDisposable
    {
        protected abstract TUnitOfWork Create(IPipelineContext context, TUnitOfWork parent = null);

        protected abstract void Commit(TUnitOfWork unitOfWork, IPipelineContext context);

        protected abstract void Rollback(TUnitOfWork unitOfWork, IPipelineContext context, ExceptionDispatchInfo exception);

        public virtual void Execute(IPipelineContext context, Action next)
        {
            var parent = context.Get<TUnitOfWork>();

            using (var uow = Create(context, parent))
            {
                context.Set(uow);

                try
                {
                    next();

                    Commit(uow, context);
                }
                catch (Exception exception)
                {
                    Rollback(uow, context, ExceptionDispatchInfo.Capture(exception));
                }
                finally
                {
                    context.Remove<TUnitOfWork>();
                }
            }
        }

        public virtual async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var parent = context.Get<TUnitOfWork>();

            using (var uow = Create(context, parent))
            {
                context.Set(uow);

                try
                {
                    await next();

                    Commit(uow, context);
                }
                catch (Exception exception)
                {
                    Rollback(uow, context, ExceptionDispatchInfo.Capture(exception));
                }
                finally
                {
                    context.Remove<TUnitOfWork>();
                }
            }
        }
    }
}
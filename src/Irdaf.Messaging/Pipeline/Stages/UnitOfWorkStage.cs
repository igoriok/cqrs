using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class UnitOfWorkStage<TUnitOfWork> : IStage
        where TUnitOfWork : class, IDisposable
    {
        private readonly Func<IPipelineContext, TUnitOfWork> _factory;
        private readonly Action<TUnitOfWork> _commit;
        private readonly Action<TUnitOfWork> _rollback;

        public UnitOfWorkStage(Func<IPipelineContext, TUnitOfWork> factory, Action<TUnitOfWork> commit, Action<TUnitOfWork> rollback)
        {
            _factory = factory;
            _commit = commit;
            _rollback = rollback;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            using (var uow = _factory(context))
            {
                context.Set(uow);

                try
                {
                    next();

                    _commit(uow);
                }
                catch
                {
                    _rollback(uow);

                    throw;
                }
                finally
                {
                    context.Set<TUnitOfWork>(null);
                }
            }
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (var uow = _factory(context))
            {
                context.Set(uow);

                try
                {
                    await next();

                    _commit(uow);
                }
                catch
                {
                    _rollback(uow);

                    throw;
                }
                finally
                {
                    context.Set<TUnitOfWork>(null);
                }
            }
        }
    }
}
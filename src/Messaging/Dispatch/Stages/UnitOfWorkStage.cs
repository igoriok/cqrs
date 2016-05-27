using System;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Dispatch.Stages
{
    public class UnitOfWorkStage : IStage
    {
        private readonly Func<IMessageContext, IUnitOfWork> _factory;

        public UnitOfWorkStage(Func<IMessageContext, IUnitOfWork> factory)
        {
            _factory = factory;
        }

        public void Execute(IMessageContext context, Action next)
        {
            using (var uow = _factory(context))
            {
                context.Set(uow);

                try
                {
                    next();

                    uow.Commit();
                }
                catch (Exception)
                {
                    uow.Rollback();

                    throw;
                }
                finally
                {
                    context.Set<IUnitOfWork>(null);
                }
            }
        }

        public async Task ExecuteAsync(IMessageContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (var uow = _factory(context))
            {
                context.Set(uow);

                try
                {
                    await next();

                    uow.Commit();
                }
                catch (Exception)
                {
                    uow.Rollback();

                    throw;
                }
                finally
                {
                    context.Set<IUnitOfWork>(null);
                }
            }
        }
    }
}
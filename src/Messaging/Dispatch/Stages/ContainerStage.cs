using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Container;

namespace Irdaf.Messaging.Dispatch.Stages
{
    public class ContainerStage : IStage
    {
        private readonly IContainer _container;

        public ContainerStage(IContainer container)
        {
            _container = container;
        }

        public void Execute(IMessageContext context, Action next)
        {
            using (var container = _container.CreateChildContainer())
            {
                context.Set(container);

                try
                {
                    next();
                }
                finally
                {
                    context.Set<IContainer>(null);
                }
            }
        }

        public async Task ExecuteAsync(IMessageContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (var container = _container.CreateChildContainer())
            {
                context.Set(container);

                try
                {
                    await next();
                }
                finally
                {
                    context.Set<IContainer>(null);
                }
            }
        }
    }
}
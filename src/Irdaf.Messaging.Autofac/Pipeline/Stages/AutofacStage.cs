using Autofac;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class AutofacStage : BaseContainerStage<ILifetimeScope>
    {
        public ILifetimeScope Scope { get; }

        public AutofacStage(ILifetimeScope scope)
        {
            Scope = scope;
        }

        protected override ILifetimeScope CreateChildContainer(IPipelineContext context, ILifetimeScope parent = null)
        {
            var scope = parent ?? Scope;

            return scope.BeginLifetimeScope();
        }
    }
}
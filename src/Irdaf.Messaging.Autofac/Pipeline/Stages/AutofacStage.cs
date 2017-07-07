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

        protected override ILifetimeScope CreateChildContainer(IPipelineContext context)
        {
            return Scope.BeginLifetimeScope();
        }
    }
}
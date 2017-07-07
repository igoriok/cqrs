using Autofac;
using Autofac.Features.Variance;

namespace Irdaf.Messaging
{
    public class MessageHandlerProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
        }
    }
}
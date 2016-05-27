using Autofac;
using Autofac.Features.Variance;
using Irdaf.Messaging.Builders;

namespace Irdaf.Messaging
{
    public class MessageHandlerProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterType<AutofacContainer>().AsImplementedInterfaces();
        }
    }
}
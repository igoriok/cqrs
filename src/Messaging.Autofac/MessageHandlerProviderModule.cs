using Autofac;
using Irdaf.Messaging.Builders;

namespace Irdaf.Messaging
{
    public class MessageHandlerProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacMessageHandlerBuilder>().AsImplementedInterfaces();
        }
    }
}
using Autofac;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging
{
    public class MessageHandlerProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacMessageHandlerProvider>().AsImplementedInterfaces();
        }
    }
}
using Irdaf.Messaging.Dispatch;

namespace Irdaf.Messaging.Behaviors
{
    public interface IServiceBehavior
    {
        void BeforeDispatch(IMessageDispatcher dispatcher, IMessage message);

        void AfterDispatch(IMessageDispatcher dispatcher, IMessage message);
    }
}
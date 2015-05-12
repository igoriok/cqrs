using System;

namespace Irdaf.Messaging.Builders
{
    public interface IMessageHandlerBuilder
    {
        object BuildMessageHandler(Type handlerType);
    }
}
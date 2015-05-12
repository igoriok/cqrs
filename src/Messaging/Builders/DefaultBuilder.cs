using System;

namespace Irdaf.Messaging.Builders
{
    public class DefaultBuilder : IMessageBuilder, IMessageHandlerBuilder
    {
        public static readonly DefaultBuilder Instance = new DefaultBuilder();

        public object BuildMessage(Type messageType)
        {
            return Activator.CreateInstance(messageType);
        }

        public object BuildMessageHandler(Type handlerType)
        {
            return Activator.CreateInstance(handlerType);
        }
    }
}
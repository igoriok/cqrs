using System;

namespace Irdaf.Messaging.Builders
{
    public interface IMessageBuilder
    {
        object BuildMessage(Type messageType);
    }
}
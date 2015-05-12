using System;

namespace Irdaf.Messaging.Dispatch
{
    public interface IMessageContext
    {
        Type MessageType { get; }

        IMessage Message { get; }

        void Execute(object handler);
    }
}
using System;

namespace Irdaf.Messaging
{
    public interface IPipelineContext : IMessageContext
    {
        IMessage Message { get; }

        Type MessageType { get; }
    }
}
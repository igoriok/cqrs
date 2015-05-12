using System;
using System.Reflection;

namespace Irdaf.Messaging.Providers
{
    public interface IHandlerRegistration
    {
        Type MessageType { get; }

        Type HandlerType { get; }

        MethodBase HandlerMethod { get; }

        object CreateHandler();
    }
}
using System;
using System.Collections.Generic;

namespace Irdaf.Messaging.Providers
{
    public interface IMessageHandlerRegistry
    {
        IEnumerable<IHandlerRegistration> RegistrationsFor(Type messageType);

        IDisposable Register(IHandlerRegistration registration);
    }
}
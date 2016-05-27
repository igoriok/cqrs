using System.Collections;
using System.Collections.Generic;

namespace Irdaf.Messaging.Providers
{
    public interface IHandlerRegistry
    {
        IEnumerable<IHandlerSource> Sources { get; }

        IEnumerable GetHandlers(IMessageContext context);
    }
}
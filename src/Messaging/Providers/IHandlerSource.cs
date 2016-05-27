using System.Collections;

namespace Irdaf.Messaging.Providers
{
    public interface IHandlerSource
    {
        IEnumerable GetHandlers(IMessageContext context);
    }
}
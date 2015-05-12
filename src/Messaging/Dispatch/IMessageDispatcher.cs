using System.Collections.Generic;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Dispatch
{
    public interface IMessageDispatcher
    {
        void Dispatch(IMessageContext context, IEnumerable<IHandlerRegistration> registrations);
    }
}
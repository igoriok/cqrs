using System.Collections.Generic;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Dispatch
{
    public class DefaultMessageDispatcher : IMessageDispatcher
    {
        public void Dispatch(IMessageContext context, IEnumerable<IHandlerRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                var handler = registration.CreateHandler();

                context.Execute(handler);
            }
        }
    }
}
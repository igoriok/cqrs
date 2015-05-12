using System.Collections.Generic;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Dispatch
{
    public class SingleThreadMessageDispatcher : IMessageDispatcher
    {
        private readonly object _lock = new object();

        public void Dispatch(IMessageContext context, IEnumerable<IHandlerRegistration> registrations)
        {
            lock (_lock)
            {
                foreach (var registration in registrations)
                {
                    var handler = registration.CreateHandler();

                    context.Execute(handler);
                }
            }
        }
    }
}
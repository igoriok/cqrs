using System.Collections;
using System.Collections.Generic;

namespace Irdaf.Messaging.Providers
{
    public class DefaultHandlerRegistry : IHandlerRegistry
    {
        public IEnumerable<IHandlerSource> Sources { get; }

        public DefaultHandlerRegistry(IEnumerable<IHandlerSource> sources)
        {
            Sources = sources;
        }

        public IEnumerable GetHandlers(IMessageContext context)
        {
            foreach (var source in Sources)
            {
                foreach (var handler in source.GetHandlers(context))
                {
                    yield return handler;
                }
            }
        }
    }
}
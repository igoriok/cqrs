using System;
using System.Collections;
using System.Collections.Generic;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Handlers
{
    public class DefaultHandlerRegistry : IHandlerRegistry
    {
        public IEnumerable<IHandlerSource> Sources { get; }

        public DefaultHandlerRegistry(IEnumerable<IHandlerSource> sources)
        {
            Sources = sources;
        }

        public IEnumerable GetHandlers(Type handlerType, IPipelineContext context)
        {
            foreach (var source in Sources)
            {
                foreach (var handler in source.GetHandlers(handlerType, context))
                {
                    yield return handler;
                }
            }
        }
    }
}
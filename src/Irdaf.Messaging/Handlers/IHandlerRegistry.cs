using System;
using System.Collections;
using System.Collections.Generic;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Handlers
{
    public interface IHandlerRegistry
    {
        IEnumerable<IHandlerSource> Sources { get; }

        IEnumerable GetHandlers(Type handlerType, IPipelineContext context);
    }
}
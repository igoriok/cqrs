using System;
using System.Collections;

namespace Irdaf.Messaging.Handlers
{
    public interface IHandlerSource
    {
        IEnumerable GetHandlers(Type handlerType, IPipelineContext context);
    }
}
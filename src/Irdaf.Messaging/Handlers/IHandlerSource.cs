﻿using System;
using System.Collections;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Handlers
{
    public interface IHandlerSource
    {
        IEnumerable GetHandlers(Type handlerType, IPipelineContext context);
    }
}
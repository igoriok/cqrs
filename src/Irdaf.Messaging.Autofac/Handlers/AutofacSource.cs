using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Irdaf.Messaging.Pipeline;

namespace Irdaf.Messaging.Handlers
{
    public class AutofacSource : IHandlerSource
    {
        public IEnumerable GetHandlers(Type handlerType, IPipelineContext context)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

            var container = context.Get<ILifetimeScope>();
            if (container != null)
            {
                return (IEnumerable)container.Resolve(enumerableType);
            }

            return Enumerable.Empty<object>();
        }
    }
}
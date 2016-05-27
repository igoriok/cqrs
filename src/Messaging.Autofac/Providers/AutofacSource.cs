using System.Collections;
using System.Collections.Generic;
using Autofac;
using Irdaf.Messaging.Builders;
using Irdaf.Messaging.Helpers;
using IContainer = Irdaf.Messaging.Container.IContainer;

namespace Irdaf.Messaging.Providers
{
    public class AutofacSource : IHandlerSource
    {
        public IEnumerable GetHandlers(IMessageContext context)
        {
            var container = (AutofacContainer)context.Get<IContainer>();
            var handlerType = HandlerTypeHelper.GetHandlerType(context.Message.GetType());
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

            foreach (var handler in (IEnumerable)container.Scope.Resolve(enumerableType))
            {
                yield return handler;
            }
        }
    }
}
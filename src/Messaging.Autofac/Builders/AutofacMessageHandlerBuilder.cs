using System;
using Autofac;

namespace Irdaf.Messaging.Builders
{
    public class AutofacMessageHandlerBuilder : IMessageHandlerBuilder
    {
        private readonly IComponentContext _context;

        public AutofacMessageHandlerBuilder(IComponentContext context)
        {
            _context = context;
        }

        public object BuildMessageHandler(Type handlerType)
        {
            return _context.Resolve(handlerType);
        }
    }
}
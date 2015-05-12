using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Irdaf.Messaging.Helpers;

namespace Irdaf.Messaging.Providers
{
    public class AutofacMessageHandlerRegistry : DefaultMessageHandlerRegistry
    {
        private readonly IComponentContext _componentContext;

        public AutofacMessageHandlerRegistry(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public override IEnumerable<IHandlerRegistration> RegistrationsFor(Type messageType)
        {
            foreach (var handler in base.RegistrationsFor(messageType))
            {
                yield return handler;
            }

            var handlerType = HandlerTypeHelper.GetHandlerType(messageType);

            foreach (var registration in _componentContext.ComponentRegistry.RegistrationsFor(new TypedService(handlerType)))
            {
                yield return new AutofacHandlerRegistration(messageType, registration, _componentContext);
            }
        }

        private class AutofacHandlerRegistration : IHandlerRegistration
        {
            private readonly Type _messageType;
            private readonly IComponentRegistration _registration;
            private readonly IComponentContext _context;

            public Type MessageType
            {
                get { return _messageType; }
            }

            public Type HandlerType
            {
                get { return _registration.Activator.LimitType; }
            }

            public MethodBase HandlerMethod
            {
                get { throw new NotImplementedException(); }
            }

            public AutofacHandlerRegistration(Type messageType, IComponentRegistration registration, IComponentContext context)
            {
                _messageType = messageType;
                _registration = registration;
                _context = context;
            }

            public object CreateHandler()
            {
                return _registration.Activator.ActivateInstance(_context, new Parameter[0]);
            }
        }
    }
}
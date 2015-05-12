using System;
using System.Collections.Generic;

namespace Irdaf.Messaging.Providers
{
    public class DefaultMessageHandlerRegistry : IMessageHandlerRegistry
    {
        private readonly List<IHandlerRegistration> _registrations;

        public DefaultMessageHandlerRegistry()
        {
            _registrations = new List<IHandlerRegistration>();
        }

        public virtual IEnumerable<IHandlerRegistration> RegistrationsFor(Type messageType)
        {
            foreach (var registration in _registrations)
            {
                if (registration.MessageType == messageType)
                {
                    yield return registration;
                }
            }
        }

        public IDisposable Register(IHandlerRegistration registration)
        {
            return new Registration(registration, _registrations);
        }

        private class Registration : IDisposable
        {
            private readonly IList<IHandlerRegistration> _list;
            private readonly IHandlerRegistration _registration;

            public Registration(IHandlerRegistration registration, IList<IHandlerRegistration> list)
            {
                _list = list;
                _registration = registration;

                _list.Add(registration);
            }

            public void Dispose()
            {
                _list.Remove(_registration);
            }
        }
    }
}
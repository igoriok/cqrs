using System;
using System.Collections.Generic;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface IEventHandlerProvider
    {
        void AddEventHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        void RemoveEventHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        IEnumerable<IEventHandler<IEvent>> GetEventHandlers(Type eventType);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface IEventHandlerProviderAsync
    {
        void AddEventHandlerAsync<TEvent>(IEventHandlerAsync<TEvent> handler) where TEvent : IEvent;

        void RemoveEventHandlerAsync<TEvent>(IEventHandlerAsync<TEvent> handler) where TEvent : IEvent;

        Task<IEnumerable<IEventHandlerAsync<IEvent>>> GetEventHandlersAsync(Type eventType, CancellationToken token);
    }
}
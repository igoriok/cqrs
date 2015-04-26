using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Services
{
    public interface ISubscriptionServiceAsync
    {
        Task<IDisposable> SubscribeAsync<TEvent>(IEventHandlerAsync<TEvent> handler, CancellationToken token) where TEvent : IEvent;
    }
}
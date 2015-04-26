using System;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Services
{
    public interface ISubscriptionService
    {
        IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
    }
}
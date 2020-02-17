using System;
using System.Threading;

namespace Irdaf.Messaging.Rebus
{
    public static class RebusContext
    {
        private static readonly AsyncLocal<global::Rebus.Pipeline.IMessageContext> Context = new AsyncLocal<global::Rebus.Pipeline.IMessageContext>();

        public static global::Rebus.Pipeline.IMessageContext Current
        {
            get => Context.Value;
            internal set => Context.Value = value;
        }

        public static IDisposable Assign(global::Rebus.Pipeline.IMessageContext context)
        {
            return new Disposable(context);
        }

        private class Disposable : IDisposable
        {
            private readonly global::Rebus.Pipeline.IMessageContext _backup;

            public Disposable(global::Rebus.Pipeline.IMessageContext value)
            {
                _backup = Current;
                Current = value;
            }

            public void Dispose()
            {
                Current = _backup;
            }
        }
    }
}
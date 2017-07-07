using System;
using System.Collections.Generic;
using System.Linq;

namespace Irdaf.Messaging
{
    public class MessageContext : IPipelineContext, IMessageContext, IDisposable
    {
        public IMessage Message { get; }

        public Type MessageType { get; }

        public IDictionary<string, object> Items { get; }

        protected MessageContext(IMessage message)
        {
            Message = message;
            MessageType = message.GetType();

            Items = new Dictionary<string, object>();
        }

        void IDisposable.Dispose()
        {
            foreach (var disposable in Items.Values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            Items.Clear();
        }
    }
}
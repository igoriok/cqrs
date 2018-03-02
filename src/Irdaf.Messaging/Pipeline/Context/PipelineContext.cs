using System;
using System.Collections.Generic;
using System.Linq;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline
{
    public abstract class PipelineContext : IPipelineContext, IDisposable
    {
        public IMessage Message { get; }

        public IMessageConvention Convention { get; }

        public IDictionary<string, object> Items { get; }

        public object Get(string key)
        {
            Items.TryGetValue(key, out var value);

            return value;
        }

        public void Set(string key, object value)
        {
            Items[key] = value;
        }

        public void Remove(string key)
        {
            Items.Remove(key);
        }

        public bool Contains(string key)
        {
            return Items.ContainsKey(key);
        }

        protected PipelineContext(IMessage message, IMessageConvention convention)
        {
            Message = message;
            Convention = convention;

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
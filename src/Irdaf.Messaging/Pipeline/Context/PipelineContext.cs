using System;
using System.Collections.Generic;
using System.Linq;

namespace Irdaf.Messaging.Pipeline
{
    public abstract class PipelineContext : IPipelineContext, IDisposable
    {
        public IPipelineContext Parent { get; }

        public IMessage Message { get; }

        public IDictionary<string, object> Items { get; }

        public object Get(string key)
        {
            if (Items.TryGetValue(key, out var value))
            {
                return value;
            }

            return Parent?.Get(key);
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

        protected PipelineContext(IMessage message, IPipelineContext parent = null)
        {
            Message = message;
            Parent = parent;

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
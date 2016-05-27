using System;
using System.Collections.Generic;
using System.Linq;

namespace Irdaf.Messaging
{
    public class MessageContext : IMessageContext, IDisposable
    {
        [ThreadStatic]
        private static MessageContext _current;

        private readonly Dictionary<string, object> _dictionary;

        public static IMessageContext Current
        {
            get { return _current; }
        }

        protected MessageContext(IMessage message)
        {
            if (_current == null)
            {
                _current = this;
                _dictionary = new Dictionary<string, object>();
            }
            else
                _dictionary = _current._dictionary;

            Message = message;
        }

        public IMessage Message
        {
            get { return Get<IMessage>("Message"); }
            protected set { Set("Message", value); }
        }

        public T Get<T>(string key)
        {
            object value;

            if (_dictionary.TryGetValue(key, out value))
                return (T)value;

            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            if (value == null)
                _dictionary.Remove(key);
            else
                _dictionary.Add(key, value);
        }

        void IDisposable.Dispose()
        {
            if (_current == this)
            {
                foreach (var disposable in _dictionary.Values.OfType<IDisposable>())
                {
                    disposable.Dispose();
                }

                _current = null;
            }
        }
    }
}
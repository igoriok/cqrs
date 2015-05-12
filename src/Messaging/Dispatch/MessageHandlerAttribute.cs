using System;

namespace Irdaf.Messaging.Dispatch
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MessageHandlerAttribute : Attribute
    {
        public bool IsMultithread { get; set; }

        public MessageHandlerAttribute()
        {
            IsMultithread = true;
        }
    }
}
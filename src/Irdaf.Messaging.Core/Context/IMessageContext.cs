using System.Collections.Generic;

namespace Irdaf.Messaging
{
    public interface IMessageContext
    {
        IDictionary<string, object> Items { get; }
    }
}
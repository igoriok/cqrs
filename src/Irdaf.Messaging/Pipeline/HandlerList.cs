using System.Collections.Generic;

namespace Irdaf.Messaging.Pipeline
{
    public class HandlerList : List<object>
    {
        public HandlerList(IEnumerable<object> handlers)
            : base(handlers)
        {
        }
    }
}
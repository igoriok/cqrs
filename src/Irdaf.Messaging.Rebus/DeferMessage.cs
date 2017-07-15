using System;

namespace Irdaf.Messaging
{
    public class DeferMessage : ICommand
    {
        public TimeSpan Delay { get; set; }

        public IMessage Message { get; set; }
    }
}
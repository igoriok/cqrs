using Irdaf.Messaging.Dispatch.Executors;

namespace Irdaf.Messaging.Dispatch
{
    public class CommandContext : MessageContext
    {
        public ICommand Command
        {
            get { return (ICommand)Message; }
        }

        public CommandContext(ICommand command)
            : base(command)
        {
            this.Set<IExecutor>(new CommandExecutor(this));
        }
    }
}
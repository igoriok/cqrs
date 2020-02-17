namespace Irdaf.Messaging.Pipeline
{
    public sealed class CommandContext : PipelineContext
    {
        public ICommand Command
        {
            get { return (ICommand)Message; }
        }

        public CommandContext(ICommand command, IPipelineContext parent = null)
            : base(command, parent)
        {
        }
    }
}
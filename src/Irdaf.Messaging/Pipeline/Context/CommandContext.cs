using System;
using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline
{
    public sealed class CommandContext : PipelineContext
    {
        public ICommand Command
        {
            get { return (ICommand)Message; }
        }

        public CommandContext(ICommand command)
            : base(command, (IMessageConvention)Activator.CreateInstance(typeof(CommandConvention<>).MakeGenericType(command.GetType())))
        {
        }
    }
}
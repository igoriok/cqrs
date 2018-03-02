using System;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public interface IStage
    {
        void Execute(IPipelineContext context, Action next);
    }
}
using Irdaf.Messaging.Pipeline.Stages;

namespace Irdaf.Messaging.Pipeline
{
    public static class ValidationExtensions
    {
        public static PipelineBuilder UseDataContractValidation(this PipelineBuilder builder)
        {
            return builder.Use<DataContractValidationStage>();
        }
    }
}
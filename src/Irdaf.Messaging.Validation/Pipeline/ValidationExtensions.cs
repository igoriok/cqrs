using System;
using Irdaf.Messaging.Pipeline.Stages;
using Irdaf.Messaging.Validation;

namespace Irdaf.Messaging.Pipeline
{
    public static class ValidationExtensions
    {
        private static readonly IValidator DataContractValidator = new DataContractValidator();

        public static PipelineBuilder UseValidation(this PipelineBuilder builder, Func<IMessageContext, IValidator> validator)
        {
            return builder.Use(ctx => new ValidationStage(validator));
        }

        public static PipelineBuilder UseDataContractValidation(this PipelineBuilder builder)
        {
            return builder.UseValidation(ctx => DataContractValidator);
        }
    }
}
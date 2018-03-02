using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Validation;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public abstract class BaseValidationStage : IStage, IStageAsync
    {
        protected abstract ValidationResult Validate(IPipelineContext context);

        protected abstract Task<ValidationResult> ValidateAsync(IPipelineContext context, CancellationToken cancellationToken);

        public void Execute(IPipelineContext context, Action next)
        {
            var result = Validate(context);

            if (result.IsValid)
            {
                next();
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var result = await ValidateAsync(context, cancellationToken);

            if (result.IsValid)
            {
                await next();
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
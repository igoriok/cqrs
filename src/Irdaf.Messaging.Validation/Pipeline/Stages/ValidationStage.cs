using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Validation;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class ValidationStage : IStage
    {
        private readonly Func<IPipelineContext, IValidator> _validator;

        public ValidationStage(Func<IPipelineContext, IValidator> validator)
        {
            _validator = validator;
        }

        public void Execute(IPipelineContext context, Action next)
        {
            var validator = _validator(context);

            if (validator == null)
            {
                throw new InvalidOperationException("Validator is null");
            }
            
            validator.Validate(context);

            next();
        }

        public async Task ExecuteAsync(IPipelineContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            var validator = _validator(context);

            if (validator == null)
            {
                throw new InvalidOperationException("Validator is null");
            }

            validator.Validate(context);

            await next();
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Validation;

namespace Irdaf.Messaging.Pipeline.Stages
{
    public class DataContractValidationStage : BaseValidationStage
    {
        protected override Validation.ValidationResult Validate(IPipelineContext context)
        {
            var validationContext = new ValidationContext(context.Message);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(context.Message, validationContext, validationResults))
            {
                return Validation.ValidationResult.Success();
            }

            var errors = validationResults
                .SelectMany(v => v.MemberNames.Select(m => new ValidationError(m, v.ErrorMessage)))
                .ToArray();

            return Validation.ValidationResult.Failed(errors);
        }

        protected override Task<Validation.ValidationResult> ValidateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() => Validate(context), cancellationToken);
        }
    }
}
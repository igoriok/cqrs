namespace Irdaf.Messaging.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; }

        public ValidationError[] Errors { get; }

        public ValidationResult()
        {
            IsValid = true;
            Errors = new ValidationError[0];
        }

        public ValidationResult(params ValidationError[] errors)
        {
            IsValid = false;
            Errors = errors ?? new ValidationError[0];
        }

        public static ValidationResult Success()
        {
            return new ValidationResult();
        }

        public static ValidationResult Failed(params ValidationError[] errors)
        {
            return new ValidationResult(errors);
        }
    }
}
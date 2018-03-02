using System;

namespace Irdaf.Messaging.Validation
{
    public class ValidationException : Exception
    {
        public ValidationError[] Errors { get; }

        public ValidationException(string message, params ValidationError[] errors)
            : base(message)
        {
            Errors = errors;
        }

        public ValidationException(params ValidationError[] errors)
            : this("Validation failed", errors)
        {
        }
    }
}
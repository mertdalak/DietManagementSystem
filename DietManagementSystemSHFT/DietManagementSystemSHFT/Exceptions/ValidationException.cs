using System;

namespace DietManagementSystemSHFT.Exceptions
{
    public class ValidationException : BadRequestException
    {
        public object Errors { get; }

        public ValidationException(string message, object errors) : base(message)
        {
            Errors = errors;
        }

        public ValidationException(string message, object errors, string errorCode) : base(message, errorCode)
        {
            Errors = errors;
        }
    }
}
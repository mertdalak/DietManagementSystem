using System;

namespace DietManagementSystemSHFT.Exceptions
{
    /// <summary>
    /// Exception thrown when there's a conflict with the current state of the resource
    /// </summary>
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, string errorCode) : base(message, errorCode)
        {
        }
    }
}
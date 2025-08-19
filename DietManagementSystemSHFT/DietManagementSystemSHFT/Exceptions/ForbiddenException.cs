using System;

namespace DietManagementSystemSHFT.Exceptions
{
    /// <summary>
    /// Exception thrown when a user is authenticated but not authorized to access a resource
    /// </summary>
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, string errorCode) : base(message, errorCode)
        {
        }
    }
}
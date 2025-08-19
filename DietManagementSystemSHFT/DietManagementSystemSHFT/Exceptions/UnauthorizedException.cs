using System;

namespace DietManagementSystemSHFT.Exceptions
{
    /// <summary>
    /// Exception thrown when a user is not authorized
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, string errorCode) : base(message, errorCode)
        {
        }
    }
}
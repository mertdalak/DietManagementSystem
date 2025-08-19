using System;

namespace DietManagementSystemSHFT.Exceptions
{
    public abstract class AppException : Exception
    {
        public string ErrorCode { get; }
        
        protected AppException(string message, string errorCode = null) : base(message)
        {
            ErrorCode = errorCode ?? GetType().Name;
        }
    }
}
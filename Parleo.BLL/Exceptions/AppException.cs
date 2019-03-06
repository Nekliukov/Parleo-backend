using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Parleo.BLL
{
    public class AppException : Exception
    {
        public ErrorType Error { get; set; }

        public AppException(ErrorType errorType)
        {
            Error = errorType;
        }

        public AppException(ErrorType errorType, string message) : base(message)
        {
            Error = errorType;
        }

        public AppException(ErrorType errorType, string message, Exception innerException) : base(message, innerException)
        {
            Error = errorType;
        }
    }
    public enum ErrorType
    {
        //throws out when Email already exists
        ExistingEmail, 

        InvalidPassword,

        InvalidId
    }
}

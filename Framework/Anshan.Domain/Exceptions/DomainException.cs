using System;

namespace Anshan.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string code, string message) : base(message)
        {
            Code = code;
        }

        public DomainException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

        public string Code { get; }

        public int StatusCode { get; }
    }
}